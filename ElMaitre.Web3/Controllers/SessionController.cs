using ElMaitre.Web.Extensions;
using ElMaitre.Web.ViewModels;
using ElMaitre.Web3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Threading;

namespace ElMaitre.Web3.Controllers
{
    public class SessionController : Controller
    {
        System.Threading.Timer timer;

        public SessionController()
        {
        }

        public IActionResult Index()
        {
            return View(new BaseViewModel(Request));

        }

        public IActionResult Call()
        {
           // timer = new System.Threading.Timer(TimerElapsed, null, new TimeSpan(0), new TimeSpan(0, 0, 10));

            return View(new BaseViewModel(Request));

        }

        private void TimerElapsed(object state)
        {
        }


    }
}
