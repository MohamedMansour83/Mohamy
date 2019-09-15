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
    public class ServiceApiController : BaseApiController
    {
        private readonly IServiceService ServiceService;
        public ServiceApiController(IServiceService ServiceService,
            UserManager<ApplicationUser> userManager) : base(userManager)
        {
            this.ServiceService = ServiceService;
        }
        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("Get/{serviceId}/{lawyerId}")]
        public IActionResult Get(int serviceId, int lawyerId)
        {
            var service = ServiceService.GetByLawyerId(serviceId, lawyerId);
            
            return Ok(service);
        }

        [Route("PayService/{serviceId}/{lawyerId}/{orderId}")]
        public IActionResult PayService(int serviceId, int lawyerId, long orderId)
        {
            ServiceService.PayService(serviceId, lawyerId, orderId);
            return Ok();
        }

        [Route("ConfirmPayment/{orderId}")]
        public IActionResult ConfirmPayment(long orderId)
        {
            var res = ServiceService.ConfirmPayment(orderId);
            return Ok(res);
        }

        [HttpGet]
        [Route("NewOrd/{serviceId}/{lawyerId}/{orderId}")]
        public IActionResult NewOrd(int serviceId, int lawyerId, int orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ServiceService.NewOrd(serviceId, lawyerId, orderId, userId);
            return Ok();
        }

        [HttpGet]
        [Route("UpdateOrd/{orderId}/{hmac}/{created_at}/{currency}/{is_refund}/{txn_response_code}/{profile_id}/{success}/{order}/{id}/{amount_cents}")]
        public IActionResult UpdateOrd(int orderId, string hmac, string created_at, 
            string currency, bool is_refund, int txn_response_code, int profile_id,
            bool success, int order, int id, int amount_cents)
        {
            ServiceService.UpdateOrd(orderId, hmac, created_at, currency, is_refund, txn_response_code,
                profile_id, success, order, id, amount_cents);
            return Ok();
        }
    }
}
