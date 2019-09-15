using ElMaitre.DAL.Data;
using ElMaitre.DTO;
using ElMaitre.Services;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.Logger;
using ElMaitre.Web.Models;
using ElMaitre.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LawyerApiController : BaseApiController
    {
        private readonly ILawyerService lawyerService;
        private readonly ILawyerAppointmentService lawyerAppointmentService;
        private readonly ISesstionService SesstionService;
        private readonly IPriceRangeService PriceRangeService;
        private readonly IDocumentService DocumentService;
        private readonly ICountryService CountryService;
        private readonly IReviewService ReviewService;
        private readonly IServiceService ServiceService;
        private readonly IEmailSender emailSender;
		ApplicationDbContext con;


		public LawyerApiController(ILawyerService lawyerService, 
            ILawyerAppointmentService lawyerAppointmentService, ISesstionService SesstionService, IPriceRangeService PriceRangeService,
            IDocumentService DocumentService, ICountryService CountryService, IReviewService ReviewService, IServiceService ServiceService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager, ILoggerManager logger, ApplicationDbContext con) :base(userManager, logger)
        {
            this.lawyerService = lawyerService;
            this.lawyerAppointmentService = lawyerAppointmentService;
            this.SesstionService = SesstionService;
            this.PriceRangeService = PriceRangeService;
            this.DocumentService = DocumentService;
            this.CountryService = CountryService;
            this.ReviewService = ReviewService;
            this.ServiceService = ServiceService;
            this.emailSender = emailSender;
			this.con = con;
		}

		[HttpPost]
        [Route("Get")]
        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([FromBody] SearchViewModel model)
        {
            var gender = (int)model.Gender;
            if (gender == -1)
                model.Gender = null;

            float minFees, maxFees = 0;
            float.TryParse(model.MinFees, out minFees);
            float.TryParse(model.MaxFees, out maxFees);

            bool? isOnline = null;
            if (model.Isonline != -1)
            {
                isOnline = model.Isonline == 1;
            }

            var res = lawyerService.GetLawyers(model.ServiceId,model.Name, model.Specialization,model.Gender,model.Rating, minFees, maxFees, model.Prices, isOnline,model.Experiences);
            
            return Ok(res);
        }

        [HttpGet]
        [Route("GetDetails/{Id}")]
        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        public IActionResult GetDetails(int Id)
        {
            var model = lawyerService.GetLawyer(Id);
            model.Services= ServiceService.Get(Id);
            return Ok(model);
        }


        [HttpGet]
        [Route("GetSpetializations")]
        //[Authorize(Roles = "Anonymous,User,Lawyer")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpetializations()
        {
            return Ok(lawyerService.GetSpetializations());
        }

        [HttpGet]
        [Route("GetPrices")]
        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        public IActionResult GetPrices()
        {
            return Ok(PriceRangeService.Get());
        }

        [HttpGet]
        [Route("GetExperiences")]
        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        public IActionResult GetExperiences()
        {
            return Ok(lawyerService.GetLawyerExperiences());
        }

        [HttpGet]
        [Route("GetAppointments/{Id}/{Date?}")]
        //[Authorize(Roles = "Lawyer,User,Anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAppointments(int Id,string Date=null)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = lawyerAppointmentService.GetAppointments(userId, Id);

            if (!string.IsNullOrEmpty(Date))
            {
                result = result.Where(s => s.Date.Date == DateTime.ParseExact(Date, "yyyy-MM-dd", CultureInfo.InvariantCulture));
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("GetAppointmentDetails/{Id}")]
        //[Authorize(Roles = "User,Anonymous")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAppointmentDetails(int Id)
       {
            var result = lawyerAppointmentService.GetAppointmentDetails(Id);
            return Ok(result);
        }

        [HttpGet]
        [Route("BookAppointment/{Id}/{orderId}")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> BookAppointment(int Id, long orderId)
        {
            var session = SesstionService.GetSesstionByAppointmentId(Id);
            if (session != null)
                return Ok(new { Message = IsLTR ? "Session is reserved." : "الاستشارة محجوزة من قبل", Success = false });
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            SesstionService.Add(new SesstionDTO { AppointmentId = Id, UserId = userId, Duration = 30, UserName = user.NameEn, UserNameAr =user.Name,
                OrderId = orderId.ToString(),
				IsPaymentConfirmed = false, merchant_order_id = orderId.ToString() });
            session = SesstionService.GetSesstionByAppointmentId(Id);
            var lawyer = UserManager.Users.FirstOrDefault(usr => usr.LawyerId == session.Appointment.LawyerId);
            await emailSender.Send(lawyer.Email, $"Reserved Session",
                $"{user.NameEn} has been reserved a session at time {session.Appointment.TimeFrom} <a href='{Url.Action("Index", "Session", new { Id = session.SessionId }, protocol: Request.Scheme)}'>go to session</a>.");
            return Ok(new { Success = true, SessionId = session.Id });
        }

		[HttpGet]
		[Route("PayAppointment/{orderId}/{amount_cents}/{hmac}/{profile_id}/{created_at}/{order}")]
		//[Authorize(Roles = "User")]
		public async Task<IActionResult> PayAppointment(string orderId, int amount_cents, string hmac, 
			int profile_id, string created_at, int order)
		{
            var session = SesstionService.GetSesstionBymerchantId(order);
            session.IsPaymentConfirmed = true;
			session.amount_cents = amount_cents;
			session.created_at = created_at;
			session.order = order;
			session.profile_id = profile_id;
			session.hmac = hmac;
            SesstionService.UpdateX(session);
            //con.SaveChanges();
            //await con.SaveChangesAsync();
            var session2 = SesstionService.GetSesstionBymerchantId(order);
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user = await UserManager.FindByIdAsync(userId);
			var lawyer = UserManager.Users.FirstOrDefault(usr => usr.LawyerId == session.Appointment.LawyerId);
			await emailSender.Send(lawyer.Email, $"Reserved Session",
				$"{user.NameEn} has been payed successfully a session at time {session.Appointment.Time.Hours} <a href='{Url.Action("Index", "Session", new { Id = session.SessionId }, protocol: Request.Scheme)}'>go to session</a>.");
			return Ok(new { Success = true, SessionId = session.Id });
		}

		[HttpPost]
        [Route("SetAppointments")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> SetAppointments([FromBody] AppointmentViewModel model)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);

            if (model.Appointments.Count==0)
            {
                model.Appointments.Add(new AppointmentModel { Date=model.Date ,Time=DateTime.Now.ToEgyptTimezone().AddHours(-2).ToString("hh:mm tt") });
            }


            var appointments = model.Appointments.Select(s => new AppointmentDTO
            {
                Date = DateTime.ParseExact(s.Date, "yyyy-MM-dd", CultureInfo.InvariantCulture).Date,
                Time = DateTime.ParseExact(s.Time, "hh:mm tt", CultureInfo.CurrentCulture).TimeOfDay,
                LawyerId = user.LawyerId.Value,
                Amount = 250
            });


            

            var result = lawyerAppointmentService.SetAppointments(appointments);
            if (!result.IsSuccess)
            {
                return BadRequest(new List<string> { result.ErrorMessage });
            }

            return Ok(result);
        }


        [HttpGet]
        [Route("GetLawyerDetails")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> GetLawyerDetails()
        {
            var vm = new UserProfileViewModel { };
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await UserManager.FindByIdAsync(userId);
                if (user == null)
                    return Unauthorized();


                vm = new UserProfileViewModel
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
                    PhoneNumber = user.PhoneNumber,
                    ProvinceId = user.ProvinceId,
                    ProfileImg = string.IsNullOrEmpty(user.ProfileImg) ? "/images/personal-img.jpg" : user.ProfileImg,
                };
                vm.Provinces = CountryService.GetProvinces();
                if (user.LawyerId.HasValue)
                {
                    var lawyer = lawyerService.GetLawyer(user.LawyerId.Value);
                    vm.Spetializations = lawyerService.GetSpetializations();
                    vm.SpetializationId = lawyer.SpecializationId;
                    vm.Sessions = SesstionService.GetLawyerSesstions(user.LawyerId.Value);
                    vm.Description = lawyer.Description;
                    vm.DescriptionEn = lawyer.DescriptionEn;
                    vm.Certificates = lawyer.Certificates;
                    vm.CertificatesEn = lawyer.CertificatesEn;
                    vm.Price = lawyer.Fees;
                    vm.Rating = lawyerService.GetRating(user.LawyerId.Value);
                    vm.ExperienceId = lawyer.ExperienceId;

                    vm.Experiences = lawyerService.GetLawyerExperiences();
                    vm.Services = ServiceService.Get(user.LawyerId.Value);
                    vm.ServiceCategories = ServiceService.GetCategories(user.LawyerId.Value);
                    vm.VideoURL = lawyer.VideoURL;


                    lawyer.ModifiedDate = DateTime.Now;
                    lawyerService.UpdateLawyer(lawyer);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return Ok(vm);
        }

        [HttpGet]
        [Route("AddServiceToLawyer/{serviceId}/{price}/{price2}")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> AddServiceToLawyer(int serviceId, int price, int price2)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            var res = ServiceService.Get(user.LawyerId.Value);
            if (res.ToList().Count > 7)
            {
                return Ok(new ServiceResponse
                {
                    Code = ServiceResponseCode.Success,
                    Message = "Exceeded the limit",
                    ArabicMessage = "تم تخطى عدد الخدمات المسموحة",
                    Data = res,
                    ResponseStatus = false
                });
            }
            else
            {
                var chk = res.FirstOrDefault(ser => ser.Id == serviceId);
                if (chk == null)
                {
                    ServiceService.AddServiceToLawyer(serviceId, user.LawyerId.Value, price, price2);
                }
                else
                {
                    ServiceService.UpdateServiceToLawyer(new DAL.Models.LawyerService
                    {
                        Id = chk.IdentId,
                        ServiceId = serviceId,
                        LawyerId = user.LawyerId.Value,
                        PriceProvided = price,
                        PriceLevel2Provided = price2
                    });
                }

                res = ServiceService.Get(user.LawyerId.Value);
                //return Ok(res);
                return Ok(new ServiceResponse
                {
                    Code = ServiceResponseCode.Success,
                    Message = "Added",
                    Data = res,
                    ResponseStatus = true
                });
            }

        }

        [HttpGet]
        [Route("RemoveServiceLawyer/{serviceId}")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> RemoveServiceLawyer(int serviceId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            ServiceService.RemoveServiceLawyer(serviceId, user.LawyerId.Value);
            var res= ServiceService.Get(user.LawyerId.Value);
            return Ok(res);
        }

        


        [HttpPost]
        [Route("ReviewReply")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> ReviewReply([FromBody] ReviewReplyViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();

            ReviewService.InsertReply(new DTO.ReplyDTO { Title = model.Reply, ReviewId = model.ReviewId, UserId = userId });

            if (userId != model.UserId)
            {
                var userWillReceiveMail = await UserManager.FindByIdAsync(model.UserId);

                await emailSender.Send(userWillReceiveMail.Email, "Someone replied to your question",
                        $"To view reply <a href='{Url.Action("LawyerProfile", "Account", new { tab = "rating" }, protocol: Request.Scheme)}'>clicking here</a>.");
            }




            return Ok(await GetLawyerDetails());
        }


        [HttpPost]
        [Route("SendNote")]
        //[Authorize(Roles = "User")]
        public async Task<IActionResult> SendNote([FromBody] SendNoteModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized();
            try
            {
                SesstionService.InsertNote(new SessionNoteDTO { SessionId = model.SessionId, Title = model.Note, UserId = userId });
            }
            catch (Exception ex)
            {

            }
            
            return Ok(new { Message = IsLTR ? "Note added successfully." : "تمت إضافة الملاحظة بنجاح." });
        }

		[HttpGet]
		[Route("GetLawyerPendingLiveSesstions")]
		[Authorize(Roles = "Lawyer")]
		public async Task<IActionResult> GetLawyerPendingLiveSesstions()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var user = await UserManager.FindByIdAsync(userId);
			var sessions = SesstionService.GetLawyerPendingLiveSesstions((int)user.LawyerId);
			sessions = sessions.Where(w => ((w.Appointment.Date.Add(w.Appointment.Time) - DateTime.Now.ToEgyptTimezone()).TotalMinutes + w.Duration) >= 0
			).ToList();
			if (sessions != null && sessions.Count() > 0)
			{
				sessions = sessions.OrderBy(s => s.Appointment.Date.Add(s.Appointment.Time));
				var session = sessions.FirstOrDefault();
				if (session != null)
					return Ok(new UserNotificationModel
					{
                        SessionId = session.SessionId,
                        UserName = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" ? session.UserName : session.UserNameAr,
                        SessionTime = session.Appointment.Date,
                        lang = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en" ? "en" : "ar"
                    });
			}
			return Ok();
		}
	}
}
