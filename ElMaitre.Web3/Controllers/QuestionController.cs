﻿using ElMaitre.Web.ViewModels;
using ElMaitre.Web3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElMaitre.Web3.Controllers
{
    //[CustomAuthorize("", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionController : Controller
    {
        public QuestionController()
        {
        }
        public IActionResult Index()
        {
            return View(new BaseViewModel(Request));

        }

        public IActionResult Details()
        {
            return View(new BaseViewModel(Request));

        }


    }
}
