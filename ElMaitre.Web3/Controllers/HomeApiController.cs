using ElMaitre.DAL.Data;
using ElMaitre.Services;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    [Route("api/[controller]")]
    public class HomeApiController : BaseApiController
    {
        private readonly ILawyerService lawyerService;
        private readonly IQuestionService QuestionService;
        private readonly IContractService ContractService;
        private readonly IServiceService ServiceService;
        private readonly IEmailSender emailSender;


        public HomeApiController(ILawyerService lawyerService, IQuestionService QuestionService,
            IServiceService ServiceService, IContractService ContractService,
            IEmailSender emailSender,
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.lawyerService = lawyerService;
            this.QuestionService = QuestionService;
            this.ServiceService = ServiceService;
            this.ContractService = ContractService;
            this.emailSender = emailSender;
        }

        //[Authorize(Roles = "Anonymous,Lawyer,User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetQuestionCategories")]
        public IActionResult GetQuestionCategories()
        {
            return Ok(QuestionService.GetCategories());
        }

        //[Authorize(Roles = "Anonymous,Lawyer,User")]
        [AllowAnonymous]
        [HttpPost]
        [Route("SendQuestion")]
        public async Task<IActionResult> SendQuestion([FromBody] SendQuestionViewModel model)
        {
            if (model.Question.Length > 200)
            {
                string msg = IsLTR ? "Question max length is 200 character" : "حجم السؤال لا يتجاوز 200 حرف";
                return BadRequest(new List<string> { msg });
            }
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            userId = User.FindFirst(ClaimTypes.Role)?.Value == "Anonymous" ? null : userId;

            int id = QuestionService.Insert(new DTO.QuestionDTO
            {
                Title = model.Question,
                CategoryId = model.CategoryId,
                UserId = userId,
                AnonymousEmail = model.AnonymousEmail,
                AnonymousName = model.AnonymousName,
            });

            string emailRes = string.Empty;
            if (!string.IsNullOrEmpty(userId))
            {
                var userWillReceiveMail = await UserManager.FindByIdAsync(userId);
                await emailSender.Send(userWillReceiveMail.Email, "Question", $"Question has been sent successfully <a href='{Url.Action("Details","Question",new { id },protocol:Request.Scheme)}'>view Question</a>");
            }
            else
            {
                await emailSender.Send(model.AnonymousEmail, "Question", $"Question has been sent successfully <a href='{Url.Action("Details", "Question", new { id }, protocol: Request.Scheme)}'>view Question</a>");
            }
            return Ok(emailRes);
        }


        [HttpGet]
        [Route("GetServiceCategories")]
        //[Authorize(Roles = "Anonymous,User,Lawyer")]
        [AllowAnonymous]
        public async Task<IActionResult> GetServiceCategories()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);
            var services = ServiceService.GetCategories(user.LawyerId);

            return Ok(services);
        }


        [HttpGet]
        [Route("GetContractCategories")]
        //[Authorize(Roles = "Anonymous,User,Lawyer")]
        [AllowAnonymous]
        public async Task<IActionResult> GetContractCategories()
        {
            var categories = ContractService.GetCategories();
            return Ok(categories);
        }


        [HttpGet]
        [Route("GetData")]
        public IActionResult GetData()
        {
            return Ok(lawyerService.GetLawyers());
        }
    }
}
