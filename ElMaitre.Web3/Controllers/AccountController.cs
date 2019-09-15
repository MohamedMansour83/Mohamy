using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElMaitre.Web3.Models;
using ElMaitre.Web.ViewModels;
using System.Net.Http;
using ElMaitre.Web.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using ElMaitre.DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using System.IO;

namespace ElMaitre.Web3.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> localizer;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        public AccountController(IStringLocalizer<AccountController> localizer,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> SignInManager)
        {
            this.localizer = localizer;
            this.userManager = userManager;
            this.SignInManager = SignInManager;
        }


        public IActionResult Login()
        {
            return View(new BaseViewModel(Request));
        }

		public IActionResult RegisterStep2(string fb_id)
		{
            var model = new BaseViewModel(Request);
            //model.FbId = fb_id;
            return View(model);
		}

		[HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var isltr = new BaseViewModel().IsLtr;
            //string apiUrl = $"http://{Request.Host}/api/Auth/Token";
            string apiUrl = $"http://{Request.Host}/api/Auth/Token";
            //return Ok(apiUrl + body);
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var bodyJS = JsonConvert.SerializeObject(model);
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenViewModel>(data);
                    //HttpContext.Session.SetString("token", result.token);
                    WriteCookies("token", result.token, result.expiration);
                    WriteCookies("IsLawyer", result.IsLawyer.ToString(), result.expiration);
                    WriteCookies("UserName", result.UserName, result.expiration);
                    WriteCookies("UserNameEn", result.UserNameEn, result.expiration);
                    return Ok(result);
                }
            }
            var msg = isltr ? "Incorrect Email or password." : "البريد الاليكتروني أو كلمة المرور غير صحيحة.";
            //unauthorized
            return StatusCode(401, msg);
        }
        public void WriteCookies(string setting, string settingValue, DateTime expiresIn)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = expiresIn;
            Response.Cookies.Append(setting, settingValue, options);
        }


        public async Task<IActionResult> UserProfile()
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/UserApi/GetUserDetails";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfileViewModel>(data);

                    var model = new ProfileViewModel(Request);
                    model.User = result;

                    return View(model);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return View("Login", new BaseViewModel(Request));
                }
            }
            return View(new ProfileViewModel(Request));
        }

        public async Task<IActionResult> LawyerProfile()
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/LawyerApi/GetLawyerDetails";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UserProfileViewModel>(data);

                    var model = new ProfileViewModel(Request);
                    model.User = result;

                    return View(model);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    //return View("Login", new BaseViewModel(Request));
                    var data = await response.Content.ReadAsStringAsync();
                    var model = new ProfileViewModel(Request);
                    model.token = token;
                    model.res = token;

                    return View(model);
                }
                else
                {
                    var ss = "";    
                }
            }
            return View(new ProfileViewModel(Request));
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
                return View();
            return null;
        }

        public async Task<IActionResult> PasswordReset(string userId, string code, string nPass, string cPass)
        {
            if (string.IsNullOrEmpty(nPass))
                return View();

            if (string.IsNullOrEmpty(nPass) || string.IsNullOrEmpty(cPass))
            {
                ViewBag.Validation = "Missing inputs";
                return View();
            }
            if (nPass != cPass)
            {
                ViewBag.Validation = "Password not match.";
                return View();
            }

            var user = await userManager.FindByIdAsync(userId);
            var result = await userManager.ResetPasswordAsync(user, code, nPass);

            if (result.Succeeded)
            {
                ViewBag.Message = "Password Changed Successfuly.";
                //todo: send mail
            }
            else
                ViewBag.Message = "Invalid request";

            return View();

        }


        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            string redirectUrl = "";
            if (provider == "Google")
                redirectUrl = Url.Action("GoogleSignIn", "Account");
            else
                redirectUrl = Url.Action("FacebookSignIn", "Account");


            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
            //}
            //else
            //{
            // var apiRequest = "http://facebook.com/dialog/oauth?client_id="
            //                 + fb_client_id + "&response_type=token&scope=email,public_profile,user_birthday&display=popup&redirect_uri=http://www.facebook.com/connect/login_success.html";

            //}
        }
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null, string code = null, string accessToken = null)
        {
            var info = await SignInManager.GetExternalLoginInfoAsync();

            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                //ViewData["ReturnUrl"] = returnUrl;
                //ViewData["LoginProvider"] = info.LoginProvider;
                //var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                //return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
                return View();
            }
        }

        public async Task<IActionResult> FacebookSignIn(string accessToken)
        {
            var res = await GetFacebookInfo(accessToken);
            return RedirectToAction("Home", "Index");
        }
        public async Task<IActionResult> GoogleSignIn(string code, string returnUrl = null)
        {
            var accessToken = await GetGoogleAccessToken(code);
            if (!string.IsNullOrEmpty(accessToken))
            {
                var info = await GetGoogleInfo(accessToken);
            }
            return RedirectToAction("Home", "Index");
        }

        private async Task<FacebookInfo> GetFacebookInfo(string accessToken)
        {
            var requestUrl = "http://graph.facebook.com/v2.9/me/"
                             + "?fields=name,picture,birthday,email,gender,first_name,last_name&access_token=" + accessToken;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<FacebookInfo>(data);
                    return result;
                }
            }

            return null;
        }



        private Task<string> GetGoogleAccessToken(string code)
        {
            string clientsecret = "uiDtfbXXqMmyugp8h40nPu6t";
            string clientid = "388788439444-hlii2thkp1np99cegpteqcethv57f8ud.apps.googleusercontent.com";
            string redirection_url = Url.Action("index", "Home", null, protocol: Request.Scheme);
            string poststring = $"grant_type=authorization_code&code={code}&client_id={clientid}&client_secret={clientsecret}&redirect_uri={redirection_url}";
            string url = "http://accounts.google.com/o/oauth2/token";
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            //Parameters = "code=" + code + "&client_id=" + client_id + "&client_secret=" + client_sceret + "&redirect_uri=" + redirect_url + "&grant_type=authorization_code";
            byte[] byteArray = Encoding.UTF8.GetBytes(poststring);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = byteArray.Length;
            Stream postStream = webRequest.GetRequestStream();
            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Close();
            WebResponse response = webRequest.GetResponse();
            postStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(postStream);
            string responseFromServer = reader.ReadToEnd();
            GoogleAccessToken serStatus = JsonConvert.DeserializeObject<GoogleAccessToken>(responseFromServer);
            if (serStatus != null)
            {
                string accessToken = string.Empty;
                accessToken = serStatus.access_token;
                return Task.FromResult(accessToken);
            }

            return null;
        }
        private async Task<GoogleInfo> GetGoogleInfo(string accessToken)
        {
            HttpClient client = new HttpClient();

            var urlProfile = "http://www.googleapis.com/oauth2/v1/userinfo?access_token=" + accessToken;

            client.CancelPendingRequests();

            HttpResponseMessage output = await client.GetAsync(urlProfile);

            if (output.IsSuccessStatusCode)
            {
                string outputData = output.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<GoogleInfo>(outputData);
            }

            return null;
        }

        public async Task<IActionResult> LogOut()
        {


            using (HttpClient client = new HttpClient())
            {
                var token = Request.Cookies["token"];
                string apiUrl = $"http://{Request.Host}/api/Auth/Logout";
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                await client.GetAsync(apiUrl);
            }

            //HttpContext.Session.SetString("token", "");
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("AnonymousToken");
            return View("Login", new BaseViewModel(Request));
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public IActionResult Forbidden()
        {
            return View(new BaseViewModel(Request));
        }

    }

    public class FacebookInfo
    {
        public class Data
        {
            public bool is_silhouette { get; set; }
            public string url { get; set; }
        }

        public class Picture
        {
            public Data data { get; set; }
        }

        public class AgeRange
        {
            public int min { get; set; }
        }

        public string name { get; set; }
        public Picture picture { get; set; }
        public AgeRange age_range { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string id { get; set; }
    }

    public class GoogleAccessToken
    {
        public string access_token { set; get; }
        public string token_type { set; get; }
        public int expires_in { set; get; }
        public string refresh_token { set; get; }
    }
    public class GoogleInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string email { get; set; }
        public string picture { get; set; }
    }
}
