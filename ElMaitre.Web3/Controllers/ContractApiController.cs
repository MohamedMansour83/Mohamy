using ElMaitre.DAL.Data;
using ElMaitre.DTO;
using ElMaitre.Services;
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
    public class ContractApiController : BaseApiController
    {
        private readonly IContractService ContractService;

        public ContractApiController(IContractService ContractService, 
            UserManager<ApplicationUser> userManager) :base(userManager)
        {
            this.ContractService = ContractService;
        }

        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        [HttpPost]
        [Route("Get")]
        public async Task<IActionResult> Get([FromBody] ContractModel model)
        {
            var res = ContractService.Get(model.CategoryId, model.Name);
            return Ok(res);
        }

        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get([FromQuery] int id)
        {
            var res = ContractService.Get(id);
            return Ok(res);
        }

        //[Authorize(Roles = "Anonymous,User")]
        //[AllowAnonymous]
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var res = ContractService.GetCategories();
            return Ok(res);
        }

    }
}
