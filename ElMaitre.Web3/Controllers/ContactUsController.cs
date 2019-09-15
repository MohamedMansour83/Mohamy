using ElMaitre.Web.ViewModels;
using ElMaitre.Web3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ElMaitre.Web3.Controllers
{
    public class ContactUsController : Controller
    {
        public ContactUsController()
        {
        }
        public IActionResult Index()
        {
            return View(new BaseViewModel(Request));
        }

    }
}
