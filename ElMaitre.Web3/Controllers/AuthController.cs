using ElMaitre.DAL.Data;
using ElMaitre.DTO;
using ElMaitre.Services;
using ElMaitre.Web.Configuration;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Helpers;
using ElMaitre.Web.Models;
using ElMaitre.Web.ViewModels;
using ElMaitre.Web3.Controllers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ElMaitre.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IStringLocalizer<AccountController> localizer;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILawyerService lawyerService;
        private readonly IEmailSender emailSender;


        public AuthController(UserManager<ApplicationUser> userManager, ILawyerService lawyerService,
            IStringLocalizer<AccountController> localizer,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager) :base(userManager)
        {
            this.lawyerService = lawyerService;
            this.localizer = localizer;
            this.roleManager = roleManager;
            this.emailSender = emailSender;
        }

        [HttpPost]
        [Route("Token")]
        public async Task<IActionResult> Token([FromBody] LoginModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.UserName);
            if (user != null && user.EmailConfirmed && await UserManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await UserManager.GetRolesAsync(user);
                var claims = new[]
                        {
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role,roles.Count>0?roles[0]:"")
                };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.SecurityKey));

                var token = new JwtSecurityToken(
                     issuer: "http://elmaitre.com",
                     audience: "http://elmaitre.com",
                     expires: DateTime.Now.AddDays(1),
                     claims: claims,
                     signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                    );

                if (user.LawyerId.HasValue)
                {
                    var lawyer = lawyerService.GetLawyer(user.LawyerId.Value);
                    lawyer.ModifiedDate = DateTime.Now;
                    lawyerService.UpdateLawyer(lawyer);
                }
                var result = new TokenViewModel
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    IsLawyer = user.LawyerId.HasValue,
                    UserName = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserName,
                    UserNameEn = !string.IsNullOrEmpty(user.NameEn) ? user.NameEn : user.UserName
                };

                WriteCookies("token", result.token, result.expiration);
                WriteCookies("IsLawyer", result.IsLawyer.ToString(), result.expiration);
                WriteCookies("UserName", result.UserName, result.expiration);
                WriteCookies("UserNameEn", result.UserNameEn, result.expiration);

                return Ok(result);

            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("TokenFB")]
        public async Task<IActionResult> TokenFB([FromBody] LoginModel model)
        {
            try
            {

                //    var user1 = lawyerService.GetLawyerByFbId(model.FbId);
                //    if (user1 != null)
                //    {
                //        var user = await UserManager.FindByIdAsync(user1.UserId);
                //        var roles = await UserManager.GetRolesAsync(user);
                //        var claims = new[]
                //                {
                //    new Claim(ClaimTypes.NameIdentifier,user.Id),
                //    new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                //    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                //    new Claim(ClaimTypes.Role,roles.Count>0?roles[0]:"")
                //};

                //        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.SecurityKey));

                //        var token = new JwtSecurityToken(
                //             issuer: "http://elmaitre.com",
                //             audience: "http://elmaitre.com",
                //             expires: DateTime.Now.AddDays(1),
                //             claims: claims,
                //             signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                //            );

                //        if (user.LawyerId.HasValue)
                //        {
                //            var lawyer = lawyerService.GetLawyer(user.LawyerId.Value);
                //            lawyer.ModifiedDate = DateTime.Now;
                //            lawyerService.UpdateLawyer(lawyer);
                //        }
                //        var res = new TokenViewModel
                //        {
                //            token = new JwtSecurityTokenHandler().WriteToken(token),
                //            expiration = token.ValidTo,
                //            IsLawyer = user.LawyerId.HasValue,
                //            UserName = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserName,
                //            UserNameEn = !string.IsNullOrEmpty(user.NameEn) ? user.NameEn : user.UserName
                //        };

                //        return Ok(res);
                //    }

                var user = UserManager.Users.Where(u => u.FbId == model.FbId).FirstOrDefault();
                //var user = await UserManager.FindByIdAsync(user1.UserId);
                var roles = await UserManager.GetRolesAsync(user);
                var claims = new[]
                        {
                        new Claim(ClaimTypes.NameIdentifier,user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.Role,roles.Count>0?roles[0]:"")
                    };

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppConfiguration.SecurityKey));

                var token = new JwtSecurityToken(
                     issuer: "http://elmaitre.com",
                     audience: "http://elmaitre.com",
                     expires: DateTime.Now.AddDays(1),
                     claims: claims,
                     signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)

                    );

                if (user.LawyerId.HasValue)
                {
                    var lawyer = lawyerService.GetLawyer(user.LawyerId.Value);
                    lawyer.ModifiedDate = DateTime.Now;
                    lawyerService.UpdateLawyer(lawyer);
                }
                var res = new TokenViewModel
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    IsLawyer = user.LawyerId.HasValue,
                    UserName = !string.IsNullOrEmpty(user.Name) ? user.Name : user.UserName,
                    UserNameEn = !string.IsNullOrEmpty(user.NameEn) ? user.NameEn : user.UserName
                };
                WriteCookies("token", res.token, res.expiration);
                WriteCookies("IsLawyer", res.IsLawyer.ToString(), res.expiration);
                WriteCookies("UserName", res.UserName, res.expiration);
                WriteCookies("UserNameEn", res.UserNameEn, res.expiration);

                return Ok(res);
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrors());

            ApplicationUser user = new ApplicationUser
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Name = model.UserName,
                NameEn = model.UserName,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                FbId = model.FbId,
                ProfileImg = "http://graph.facebook.com/" + model.FbId + "/picture?type=large"
            };
            var result = await UserManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                string Temp = result.Errors.Select(s => s.Code).FirstOrDefault();
                if(Temp == "DuplicateUserName")
                    return BadRequest("هذا المستخدم موجود من قبل .");
                else
                    return BadRequest(result.Errors.Select(s => s.Description));
            }
                

            if (model.IsLawyer)
            {
                var id = lawyerService.InsertLawyer(new LawyerDTO { UserId = user.Id, Name = user.UserName });
                user.LawyerId = id;
                await UserManager.UpdateAsync(user);
                await UserManager.AddToRoleAsync(user, "Lawyer");
                //var lawyer = lawyerService.GetLawyerByUserId(user.Id);
                //lawyer.ProfileImg = "http://graph.facebook.com/" + user.FbId + "/picture?type=large";
                //lawyerService.UpdateLawyer(lawyer);
            }
            else
            {
                await UserManager.AddToRoleAsync(user, "User");
            }

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(
                "ConfirmEmail",
                "Account",
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);

            if (string.IsNullOrWhiteSpace(model.FbId))
            {

				//await emailSender.Send(user.Email, "Confirm your email",
				//$"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


				//var msg = IsLTR ? "Email confirmation has been sent to your email."
				//    : "تم إرسال رسالة تأكيد بالبريد الإلكتروني إلى بريدك الإلكتروني.";

				//return Ok(new { Message = msg });

				var msg = IsLTR ? "Registered on system."
						: "تم تسجيلك بنجاح.";
				user.EmailConfirmed = true;
				var result1 = await UserManager.UpdateAsync(user);


				string apiUrl = $"http://{Request.Host}/api/Auth/Token";
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri(apiUrl);
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

					var bodyJS = JsonConvert.SerializeObject(new LoginModel { UserName = model.Email, Password = model.Password });
					var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await client.PostAsync(apiUrl, body);
					if (response.IsSuccessStatusCode)
					{
						var data = await response.Content.ReadAsStringAsync();
						var result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenViewModel>(data);
						result2.Message = msg;
						//HttpContext.Session.SetString("token", result.token);
						WriteCookies("token", result2.token, result2.expiration);
						WriteCookies("IsLawyer", result2.IsLawyer.ToString(), result2.expiration);
						WriteCookies("UserName", result2.UserName, result2.expiration);
						WriteCookies("UserNameEn", result2.UserNameEn, result2.expiration);
						return Ok(result2);
					}
				}


				return Ok(new { Message = msg });
			}
            else
            {
                var msg = IsLTR ? "Registered on system."
                    : "تم تسجيلك بنجاح.";
                user.EmailConfirmed = true;
                var result1 = await UserManager.UpdateAsync(user);


                string apiUrl = $"http://{Request.Host}/api/Auth/Token";
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    var bodyJS = JsonConvert.SerializeObject(new LoginModel { UserName = model.Email, Password = model.Password });
                    var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        var result2 = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenViewModel>(data);
                        result2.Message = msg;
                        //HttpContext.Session.SetString("token", result.token);
                        WriteCookies("token", result2.token, result2.expiration);
                        WriteCookies("IsLawyer", result2.IsLawyer.ToString(), result2.expiration);
                        WriteCookies("UserName", result2.UserName, result2.expiration);
                        WriteCookies("UserNameEn", result2.UserNameEn, result2.expiration);
                        return Ok(result2);
                    }
                }


                return Ok(new { Message = msg });
            }
        }
		
		public void WriteCookies(string setting, string settingValue, DateTime expiresIn)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = expiresIn;
            Response.Cookies.Append(setting, settingValue, options);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.UserName);
            if (user == null)
                return BadRequest(new List<string>() { IsLTR ? "Invalid Email" : "بريد إلكتروني خاطئ" });

            var code = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                "PasswordReset",
                "Account",
                values: new { userId = user.Id, code = code },
                protocol: Request.Scheme);


            await emailSender.Send(user.Email, "Password Reset",
            $"Please reset your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            var msg = IsLTR ? "Email password has been sent to your email."
    : "تم إرسال كلمة مرور البريد الإلكتروني إلى بريدك الإلكتروني.";
            return Ok(new { Message = msg });
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await UserManager.FindByIdAsync(userId);

            var lawyer = lawyerService.GetLawyer(user.LawyerId.Value);
            lawyer.ModifiedDate = DateTime.Now.AddDays(-1);
            lawyerService.UpdateLawyer(lawyer);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("LoginGoogle")]
        public async Task<IActionResult> LoginGoogle()
        {
            string clientid = "388788439444-hlii2thkp1np99cegpteqcethv57f8ud.apps.googleusercontent.com";
            string redirection_url = Url.Action("signin-google", "api/auth", null, protocol: Request.Scheme);



            string url = "http://accounts.google.com/o/oauth2/v2/auth?scope=profile&include_granted_scopes=true&redirect_uri=" + redirection_url + "&response_type=code&client_id=" + clientid + "";
            return Redirect(url);
            // await HttpContext.ChallengeAsync("Google", new AuthenticationProperties() { RedirectUri = "/Account/signin-google" });
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("signin-google")]
        public async Task<IActionResult> ExternalLoginCallback(string code, string returnUrl = null)
        {

            var accessToken = await GetGoogleAccessToken(code);
            if (!string.IsNullOrEmpty(accessToken))
            {
                var info = await GetUserInfo(accessToken);
                if (info != null)
                {
                    var existsUser = await UserManager.FindByEmailAsync(info.email);

                    if (existsUser==null)
                    {
                        ApplicationUser user = new ApplicationUser
                        {
                            Email = info.email,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            UserName = info.name,
                            Gender = Gender.Male
                        };
                        var result = await UserManager.CreateAsync(user);
                        if (!result.Succeeded)
                            return BadRequest(result.Errors.Select(s => s.Description));
                    }
                    else
                    {

                    }
                    
                }
            }

            //var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            //if (result.Succeeded)
            //{
            //    return Redirect(returnUrl);
            //}
            return BadRequest();
        }








        public class GoogleAccessToken
        {
            public string access_token { set; get; }
            public string token_type { set; get; }
            public int expires_in { set; get; }
            public string refresh_token { set; get; }
        }
        public class UserInfo
        {
            public string id { get; set; }
            public string name { get; set; }
            public string given_name { get; set; }
            public string email { get; set; }
            public string picture { get; set; }
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

        private async Task<UserInfo> GetUserInfo(string token)
        {
            HttpClient client = new HttpClient();

            var urlProfile = "http://www.googleapis.com/oauth2/v1/userinfo?access_token=" + token;

            client.CancelPendingRequests();

            HttpResponseMessage output = await client.GetAsync(urlProfile);

            if (output.IsSuccessStatusCode)
            {
                string outputData = output.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<UserInfo>(outputData);
            }

            return null;
        }
    }
}