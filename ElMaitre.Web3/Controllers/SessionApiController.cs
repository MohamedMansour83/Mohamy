using ElMaitre.DAL.Data;
using ElMaitre.DTO;
using ElMaitre.Services;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Logger;
using ElMaitre.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    [Route("api/[controller]")]
    public class SessionApiController : BaseApiController
    {
        private readonly ISesstionService SesstionService;
        private readonly ILawyerService LawyerService;
        private readonly IDocumentService DocumentService;

        public SessionApiController( ISesstionService SesstionService, IDocumentService DocumentService, ILawyerService LawyerService,
            UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment,
            ILoggerManager logger) :base(userManager, hostingEnvironment, logger)
        {
            this.SesstionService = SesstionService;
            this.DocumentService = DocumentService;
            this.LawyerService = LawyerService;
        }

		//[Authorize]
		[HttpGet]
        [Route("GetSession/{Id}")]
        public async Task<IActionResult> GetSession(string Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            var session = SesstionService.GetSesstionById(Id);
            if (session.Id > 0)
            {
                if (session.UserId != userId && session.Appointment.LawyerId != user.LawyerId)
                    return BadRequest(new List<string> { "Invalid User" });
            }
            session.IsGeust = session.UserId == userId;
            session.UserName = user.UserName;

            if (user.LawyerId.HasValue)
            {
                var lawyer = LawyerService.GetLawyer(user.LawyerId.Value);
                lawyer.ModifiedDate = DateTime.Now;
                LawyerService.UpdateLawyer(lawyer);
            }


            return Ok(session);
        }


		[Authorize]
		[HttpGet]
        [Route("EndSession/{Id}")]
        public async Task<IActionResult> EndSession(string Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var session = SesstionService.EndSession(Id);
            session.IsGeust = session.UserId == userId;
            return Ok(session);
        }

		[Authorize]
		[HttpGet]
        [Route("StartSession/{Id}")]
        public async Task<IActionResult> StartSession(string Id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            var session = SesstionService.GetSesstionById(Id);
            if (session.Id > 0)
            {
                if (session.UserId != userId && session.Appointment.LawyerId != user.LawyerId)
                    return BadRequest(new List<string> { "Invalid User" });
                //session.IsStarted = true;
                session.StartedDate = DateTime.Now;
                SesstionService.Update(session);

                if (user.LawyerId.HasValue)
                {
                    var lawyer = LawyerService.GetLawyer(user.LawyerId.Value);
                    lawyer.ModifiedDate = DateTime.Now;
                    LawyerService.UpdateLawyer(lawyer);
                }

                return Ok(new { IsStarted = true });
            }

            return Ok(session);

        }

		[Authorize]
		[HttpGet]
        [Route("ConfirmPayment/{orderId}")]
        public async Task<IActionResult> ConfirmPayment(string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            var res = SesstionService.ConfirmPayment(orderId);

            if (!res)
                logger.LogError($"order: {orderId} payment faild/ UserId: {userId}");
            else
                logger.LogError($"order: {orderId} payment approved/ UserId: {userId}");


            return Ok(res);

        }

		[Authorize]
		[HttpPost]
        [Route("SendRate")]
        public async Task<IActionResult> SendRate([FromBody] ReviewViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            SesstionService.RateSession(model.SessionId, userId, model.Rate, model.Review);
            return Ok();

            return BadRequest(new List<string> { "Somthing went wrong!!!" });

        }

		[Authorize]
		[HttpPost]
        [Route("UploadFile")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file, string sessionId)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await UserManager.FindByIdAsync(userId);
                var session = SesstionService.GetSesstionById(sessionId);
                string toUserId = "";
                if (user.LawyerId.HasValue)
                {
                    toUserId = session.UserId;
                }
                else
                {
                    toUserId = LawyerService.GetLawyer(session.Appointment.LawyerId).UserId;
                }
                string fileName = $"{ Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid().ToString()}{ Path.GetExtension(file.FileName) }";
                var path = Path.Combine("uploads", fileName).ToLower();
                var fullPath = Path.Combine(hostingEnvironment.WebRootPath, path).ToLower();

                using (var fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                DocumentService.Insert(new DocumentDTO { Name = file.FileName, FromUserId = userId, ToUserId = toUserId, Path = "/" + path });

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                //System.IO.File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath,"exception.txt"), ex.Message);
                //System.IO.File.WriteAllText(Path.Combine(hostingEnvironment.WebRootPath, "stacktrace.txt"), ex.StackTrace);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("CheckSession/{Id}")]
        public IActionResult CheckSession(string Id)
        {
            var session = SesstionService.GetSesstionById(Id);
            if (session.IsPaymentConfirmed)
                return Ok(session);
            else
                return Unauthorized();
        }


		[AllowAnonymous]
		[HttpGet]
		[Route("GetPendingLiveSesstions/{Id}")]
		public IActionResult GetPendingLiveSesstions(string Id)
		{
			var sessions = SesstionService.GetPendingLiveSesstions(Id);
			return Ok(sessions);
		}
	}
}
