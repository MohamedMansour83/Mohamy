using ElMaitre.DAL.Data;
using ElMaitre.DTO;
using ElMaitre.Services;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.Models;
using ElMaitre.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    public class QuestionApiController : BaseApiController
    {
        private readonly IQuestionService QuestionService;
        private readonly IEmailSender emailSender;


        public QuestionApiController(IQuestionService QuestionService,
            IEmailSender emailSender,
        UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.QuestionService = QuestionService;
            this.emailSender = emailSender;
        }

        [HttpPost]
        [Route("Get")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> Get([FromBody] QuestionFilter model)
        {
            if (model.LawyerQuestions)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await UserManager.FindByIdAsync(userId);
                model.LawyerId = user.LawyerId;
            }


            var res = QuestionService.Get(model);
            return Ok(res);
        }

        [HttpGet]
        [Route("GetDetails/{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDetails(int Id)
        {
            var res = QuestionService.GetById(Id);
            return Ok(res);
        }

        [HttpPost]
        [Route("Reply")]
        [Authorize(Roles = "Lawyer")]
        public async Task<IActionResult> Reply([FromBody] ReplyQuestionModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var question = QuestionService.GetById(model.QuestionId);

            QuestionService.InsertAnswer(new QuestionAnswerDTO { QuestionId = model.QuestionId, Title = model.Answer, UserId = userId });

            var emailToSend = question.AnonymousEmail;
            if (!string.IsNullOrEmpty(question.UserId))
            {
                var usr = await UserManager.FindByIdAsync(question.UserId);
                emailToSend = usr.Email;
            }

            await emailSender.Send(emailToSend, "Question Reply", $"You have replay for your question <a href='{Url.Action("Details", "Question", new { Id = model.QuestionId }, protocol: Request.Scheme)}'>View Reply</a>");

            return Ok(QuestionService.Get(new QuestionFilter { UnAnswerd = model.UnAnswerQuestions }));
        }

    }
}
