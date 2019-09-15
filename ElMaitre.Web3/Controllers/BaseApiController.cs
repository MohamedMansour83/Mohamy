using ElMaitre.DAL.Data;
using ElMaitre.Web.Logger;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    public class BaseApiController : Controller
    {
       protected UserManager<ApplicationUser> UserManager;
       protected readonly IHostingEnvironment hostingEnvironment;
       protected readonly ILoggerManager logger;


        public BaseApiController(UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment, ILoggerManager logger):this(userManager, hostingEnvironment)
        {
            this.logger = logger;
        }

        public BaseApiController(UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment):this(userManager)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public BaseApiController(UserManager<ApplicationUser> userManager, ILoggerManager logger):this(userManager)
        {
            this.logger = logger;
        }

        public BaseApiController(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
        }

        public bool IsLTR { get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en";
            }
        }


    }
}
