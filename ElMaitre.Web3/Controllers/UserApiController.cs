using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using ElMaitre.DAL.Data;
using ElMaitre.Services;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.Models;
using ElMaitre.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UserApiController : BaseApiController
    {
        private readonly ISesstionService SesstionService;
        private readonly ILawyerService LawyerService;
        private readonly IReviewService ReviewService;
        private readonly IQuestionService QuestionService;
        private readonly IDocumentService DocumentService;
        private readonly ICountryService CountryService;

        public UserApiController(ISesstionService SesstionService, ILawyerService LawyerService, IReviewService ReviewService, IDocumentService DocumentService,
            IQuestionService QuestionService, ICountryService CountryService,
            UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment) : base(userManager, hostingEnvironment)
        {
            this.SesstionService = SesstionService;
            this.LawyerService = LawyerService;
            this.ReviewService = ReviewService;
            this.QuestionService = QuestionService;
            this.CountryService = CountryService;
            this.DocumentService = DocumentService;
        }


        [HttpGet]
        [Route("GetUserDetails")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetUserDetails()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();


            var vm = new UserProfileViewModel
            {
                Id = userId,
                Age = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.CalculateAge() : 0,
                Email = user.Email,
                Name = user.Name,
                NameEn = user.NameEn,
                UserName = user.UserName,
                LawyerId = user.LawyerId,
                DateOfBirth = user.DateOfBirth.HasValue ? user.DateOfBirth.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) : "",
                Gender = user.Gender,
                ProvinceId = user.ProvinceId,
                PhoneNumber = user.PhoneNumber,
                ProfileImg = string.IsNullOrEmpty(user.ProfileImg) ? "/images/personal-img.jpg" : user.ProfileImg,
            };
            vm.Documents = DocumentService.Get(userId, true);
            vm.Sessions = SesstionService.GetSesstions(userId);
            vm.Provinces = CountryService.GetProvinces();
            return Ok(vm);
        }

        [HttpGet]
        [Route("GetDocuments")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetDocuments(bool isSent)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var docs = DocumentService.Get(userId, isSent);
            var model = docs.Select(s => new DocumentModel { Name = s.Name, Date = s.Date, Path = s.Path });
            return Ok(model);
        }

        [HttpGet]
        [Route("GetNotifications")]
        [Authorize(Roles = "User")]
        //public async Task<IActionResult> GetNotifications()
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var sessions = SesstionService.GetSesstions(userId);
        //    sessions = sessions.Where(w => w.Appointment.Date.Add(w.Appointment.Time) > DateTime.Now.ToEgyptTimezone());
        //    if (sessions != null && sessions.Count() > 0)
        //    {
        //        sessions = sessions.OrderBy(s => s.Appointment.Date.Add(s.Appointment.Time));

        //        var session = sessions.FirstOrDefault();
        //        if (session != null)
        //            return Ok(new UserNotificationModel
        //            {
        //                SessionId = session.SessionId,
        //                LawyerName = session.Appointment.LawyerName,
        //                LawyerNameEn = session.Appointment.LawyerNameEn,
        //                RemainingSeconds = Convert.ToInt32(session.Appointment.Date.Add(session.Appointment.Time).Subtract(DateTime.Now.ToEgyptTimezone()).TotalSeconds)
        //            });
        //    }
        //    return Ok();
        //}

        [HttpGet]
        [Route("GetPendingLiveSesstions")]
        [Authorize(Roles = "User")]
        public IActionResult GetPendingLiveSesstions()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var sessions = SesstionService.GetPendingLiveSesstions(userId);
            sessions = sessions.Where(w => ((w.Appointment.Date.Add(w.Appointment.Time) - DateTime.Now.ToEgyptTimezone()).TotalMinutes + w.Duration) >= 0);
            if (sessions != null && sessions.Count() > 0)
            {
                sessions = sessions.OrderBy(s => s.Appointment.Date.Add(s.Appointment.Time));

                var session = sessions.FirstOrDefault();
                if (session != null)
                    return Ok(new UserNotificationModel
                    {
                        SessionId = session.SessionId,
                        LawyerName = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" ? session.Appointment.LawyerNameEn : session.Appointment.LawyerName,
                        SessionTime =session.Appointment.Date.Add(session.Appointment.Time),
                        lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" ? "en" : "ar"
                    });
            }
            return Ok();
        }

        [HttpPost]
        [Route("SaveInfo")]
        [Authorize(Roles = "User,Lawyer")]
        public async Task<IActionResult> SaveInfo([FromBody] UserProfileViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            user.Name = model.Name;
            user.NameEn = model.NameEn;
            user.DateOfBirth = DateTime.ParseExact(model.DateOfBirth, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            user.Gender = model.Gender;
            user.PhoneNumber = model.PhoneNumber;
            user.ProvinceId = model.ProvinceId;
            await UserManager.UpdateAsync(user);

            if (user.LawyerId.HasValue)
            {
                var lawyer = LawyerService.GetLawyer(user.LawyerId.Value);
                lawyer.Description = model.Description;
                lawyer.Certificates = model.Certificates;
                lawyer.DescriptionEn = model.DescriptionEn;
                lawyer.CertificatesEn = model.CertificatesEn;
                lawyer.SpecializationId = model.SpetializationId;
                lawyer.UserId = userId;
                lawyer.Fees = model.Price;
                lawyer.ExperienceId = model.ExperienceId;
                lawyer.VideoURL = model.VideoURL;

                LawyerService.UpdateLawyer(lawyer);
            }

            WriteCookies("UserName", user.Name, DateTime.Now.AddHours(2));
            WriteCookies("UserNameEn", user.NameEn, DateTime.Now.AddHours(2));


            return Ok(new { isSuccess = true });
        }





        [HttpPost]
        [Route("ChangePassword")]
        [Authorize(Roles = "Lawyer,User")]

        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrors());



            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            var res = await UserManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
            if (!res.Succeeded)
                return BadRequest(res.Errors.Select(s => s.Description));


            return Ok(res);
        }

        [HttpPost]
        [Route("UploadProfileImg")]
        public async Task<IActionResult> UploadProfileImg([FromForm] IFormFile file)
        {
            //try
            //{
            //    if (file.ContentType != "image/jpg" && file.ContentType != "image/png" && file.ContentType != "image/jpeg")
            //        return BadRequest(new List<string> { IsLTR ? "only image file supported" : "برجاء اختيار صورة" });


            //    string path = string.Empty;
            //    try
            //    {
            //        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //        var user = await UserManager.FindByIdAsync(userId);

            //        string fileName = $"{ Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid().ToString()}{ Path.GetExtension(file.FileName) }";
            //        //path = Path.Combine("uploads/profile-images", fileName).ToLower();
            //        path = Path.Combine("App_Data", fileName).ToLower();
            //        var fullPath = Path.Combine(hostingEnvironment.WebRootPath, path).ToLower();

            //        using (var fileStream = new FileStream(fullPath, FileMode.Create))
            //        {
            //            await file.CopyToAsync(fileStream);
            //        }

            //        user.ProfileImg = path;
            //        var res = await UserManager.UpdateAsync(user);
            //        if (res.Succeeded)
            //            return Ok("/" + user.ProfileImg);

            //    }
            //    catch (Exception ex)
            //    {
            //        return Ok(ex.Message);
            //        //return BadRequest(new List<string> { IsLTR ? "Something went wrong!!!" : "حدث خطأ ما!!!" });
            //    }
            //    return BadRequest(new List<string> { IsLTR ? "Something went wrong!!!" : "حدث خطأ ما!!!" });
            //}
            //catch (Exception ex)
            //{
            //    return Ok(ex.Message);
            //}

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await UserManager.FindByIdAsync(userId);

                int index = file.FileName.LastIndexOf('.');
                int index2 = file.FileName.Length - index;
                string Ext = file.FileName.Substring(index, index2);
                string Name = file.FileName.Substring(0, index);

                var newName = string.Format("{0}_{1}{2}", Name, Utilities.GetUniqueKeyNums(5), Ext);
                var path = string.Format("http://s3.eu-west-3.amazonaws.com/elasticbeanstalk-eu-west-3-937138779294/{0}",
                    newName);
                user.ProfileImg = path;
                await UserManager.UpdateAsync(user);

                FormFile file2 = null;
                //var path2 = Path.Combine(
                //           Directory.GetCurrentDirectory(),
                //           "wwwroot/App_Data", newName);
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    file2 = new FormFile(stream, 0, stream.Length, Name, newName);
                    await UploadFileToS3(file2);
                }

                return Ok(path);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        //S3 storage util
        public async Task UploadFileToS3(IFormFile file)
        {
            //using (var client = new AmazonS3Client("yourAwsAccessKeyId", "yourAwsSecretAccessKey", RegionEndpoint.USEast1))
            using (var client = new AmazonS3Client("AKIAIIDPPHBSMOAPXSFQ", "/Ex96l3s1n/U1SoyVG1geg3bPI5+bTJSbsbOANFu", RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = file.FileName,
                        BucketName = "elasticbeanstalk-eu-west-3-937138779294",
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }

        public void WriteCookies(string setting, string settingValue, DateTime expiresIn)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = expiresIn;
            Response.Cookies.Append(setting, settingValue, options);
        }



    }
}
