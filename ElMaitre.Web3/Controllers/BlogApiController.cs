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
    public class BlogApiController : BaseApiController
    {
        private readonly IBlogService BlogService;

        public BlogApiController(IBlogService BlogService, 
            UserManager<ApplicationUser> userManager) :base(userManager)
        {
            this.BlogService = BlogService;
        }

        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            var res = BlogService.Get();
            return Ok(res);
        }

        //[Authorize(Roles = "Anonymous,User")]
        [AllowAnonymous]
        [HttpGet]
        [Route("GetById")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var res = BlogService.Get(id);
            return Ok(res);
        }


    }
}
