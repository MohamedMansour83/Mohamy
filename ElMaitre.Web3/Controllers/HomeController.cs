using ElMaitre.DAL.Data;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Filters;
using ElMaitre.Web.Models;
using ElMaitre.Web.ViewModels;
using ElMaitre.Web3.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ElMaitre.Web3.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : BaseController
    {
        //private readonly IHtmlLocalizer<HomeController> _localizer;
        private readonly IStringLocalizer<HomeController> localizer;
        private readonly UserManager<ApplicationUser> userManager;
        public HomeController(IStringLocalizer<HomeController> localizer,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.localizer = localizer;
        }
        
        //public static SmtpClient InfoClient()
        //{
        //    System.Net.NetworkCredential networkCredentials = new
        //    System.Net.NetworkCredential("info@mohamy.co", "Ayman1988@");

        //    SmtpClient smtpClient = new SmtpClient();
        //    setStmpPro(smtpClient, networkCredentials);

        //    return smtpClient;
        //}

        //static void setStmpPro(SmtpClient smtpClient, System.Net.NetworkCredential networkCredentials)
        //{
        //    smtpClient.UseDefaultCredentials = false;
        //    smtpClient.Credentials = networkCredentials;
        //    smtpClient.Host = "smtp.office365.com";
        //    smtpClient.Port = 587;
        //    //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    smtpClient.EnableSsl = true;
        //    smtpClient.ServicePoint.MaxIdleTime = 1;
        //}

        public async Task<IActionResult> Index()
        {
			//try
			//{
			//    MailMessage mailMessage = new MailMessage();
			//    mailMessage.From = new MailAddress("info@mohamy.co");
			//    mailMessage.To.Add("heat9070@gmail.com");
			//    mailMessage.To.Add("magamaleldin@gmail.com");
			//    mailMessage.To.Add("gameldin@icould.com");
			//    mailMessage.Subject = "Almohamy - Gendy Test";
			//    mailMessage.IsBodyHtml = true;
			//    mailMessage.Body = "Hi from Gendy";

			//    SmtpClient smtpClient = InfoClient();
			//    smtpClient.Send(mailMessage);
			//}
			//catch (Exception ex)
			//{

			//}

			//var user = await userManager.FindByIdAsync("d2e877bc-178f-4ea0-8a24-f9fa40b49005");
			//////await userManager.RemovePasswordAsync(user);
			//////await userManager.AddPasswordAsync(user, "Ayman1988@");
			//string code = await userManager.GeneratePasswordResetTokenAsync(user);
			//IdentityResult result1 = await userManager.ResetPasswordAsync(user, code, "Ayman1988@");

			//var user = await userManager.FindByIdAsync("a3b8a446-3482-4bbe-8b9d-18a73857e18b");
			//////await userManager.RemovePasswordAsync(user);
			//////await userManager.AddPasswordAsync(user, "Ayman1988@");
			//string code = await userManager.GeneratePasswordResetTokenAsync(user);
			//IdentityResult result1 = await userManager.ResetPasswordAsync(user, code, "Ayman1988@");

			var model = new HomeViewModel(Request);
            if (!Request.Cookies.Any(s => s.Key == "token") && !Request.Cookies.Any(s => s.Key == "AnonymousToken") )
            {
                string apiUrl = $"http://{Request.Host}/api/Auth/Token";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var bodyJS = JsonConvert.SerializeObject(new LoginModel { UserName = "anonymous@gmail.com", Password = "Anonymous@P@$$w0rd123" });
                    var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenViewModel>(data);
                        //HttpContext.Session.SetString("token", result.token);
                        WriteCookies("AnonymousToken", result.token, result.expiration);
                        model.Token = result.token;
                        return View(model);
                    }
                }
            }
            model.Token = Request.Cookies.Any(s => s.Key == "token") ? Request.Cookies["token"] : Request.Cookies["AnonymousToken"];
            //model.IsLawyer = Request.Cookies.Any(s => s.Key == "IsLawyer") ? bool.Parse(Request.Cookies["IsLawyer"].ToString()) : false;
            //model.UserName = Request.Cookies.Any(s => s.Key == "UserName") ? Request.Cookies["UserName"] : "";

            return View(model);
        }


        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void WriteCookies(string setting, string settingValue, DateTime expiresIn)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = expiresIn;
            Response.Cookies.Append(setting, settingValue, options);
        }
    }
}
