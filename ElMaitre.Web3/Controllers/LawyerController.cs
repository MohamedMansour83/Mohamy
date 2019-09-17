using ElMaitre.DAL.Data;
using ElMaitre.DAL.Models;
using ElMaitre.DTO;
using ElMaitre.Web.Extensions;
using ElMaitre.Web.Filters;
using ElMaitre.Web.Helpers;
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
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElMaitre.Web3.Controllers
{
    //[CustomAuthorize("", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LawyerController : Controller
    {
        private AppSettings AppSettings { get; set; }
		ApplicationDbContext con;
		public LawyerController(IOptions<AppSettings> settings, ApplicationDbContext con)
		{
            AppSettings = settings.Value;
			this.con = con;
		}
		public IActionResult Index()
        {
            return View(new BaseViewModel(Request));
        }

        public IActionResult SelectLawyer()
        {
            return View(new BaseViewModel(Request));
        }

        public IActionResult Details()
        {
            return View(new BaseViewModel(Request));
        }

		[Route("/Lawyer/Booking")]
		public async Task<IActionResult> Booking(string hmac, string merchant_order_id,
			string created_at, string currency, bool is_refund, int txn_response_code, int profile_id,
			bool success, int order, int id, int amount_cents)
		{
            
			if (!string.IsNullOrWhiteSpace(hmac))
			{
				var token = Request.Cookies["token"];
				if (!string.IsNullOrWhiteSpace(token))
				{
					string apiUrl = $"http://{Request.Host}/api/LawyerApi/PayAppointment/{merchant_order_id}/{amount_cents}/{hmac}/{profile_id}/{created_at}/{order}";
					using (HttpClient client = new HttpClient())
					{
						client.BaseAddress = new Uri(apiUrl);
						client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
						client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

						HttpResponseMessage response = await client.GetAsync(apiUrl);
						if (response.IsSuccessStatusCode)
						{
							var data = await response.Content.ReadAsStringAsync();
							var result = JsonConvert.DeserializeObject<BookSessionResult>(data);

						}
						else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
						{
							return null;
						}
					}
				}
			}
			var bm = new BaseViewModel(Request);
			bm.Session = con.Sessions.FirstOrDefault(o => o.merchant_order_id == order.ToString());

            return View(bm);
		}

		[Route("/Lawyer/Booking/{lawyerId}/{AppointmentId}")]
        public IActionResult Booking(int lawyerId = 0, int AppointmentId = 0)
        {
            return View(new BaseViewModel(Request));
        }

        public async Task<IActionResult> BookingSuccessful(int txn_response_code, bool success, long order, double amount_cents)
        {
            if (txn_response_code == 0 && success)
            {
                var res = await ConfirmBooking(order);
                if (!res)
                    return null;
            }
            return View(new BaseViewModel(Request));
        }

        public IActionResult Questions()
        {
            return View(new BaseViewModel(Request));

        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<PaymentTokenResult> GetToken()
        {
            string apiUrl = "https://accept.paymobsolutions.com/api/auth/tokens";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var paymentApiKey = AppSettings.PaymentApiKey;


                var bodyJS = JsonConvert.SerializeObject(new { api_key = paymentApiKey });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PaymentTokenResult>(data);
                    return result;
                }
            }
            return null;
        }

        private async Task<OrderResult> OrderCreation(string token, long merchantId, double amount, int integration_Id,DeliveryDetailsModel model)
        {
            string apiUrl = $"https://accept.paymobsolutions.com/api/ecommerce/orders";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var ordId = Utilities.GetUniqueKeyNums(6);
                order_id = int.Parse(ordId);

                //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var bodyJS = JsonConvert.SerializeObject(new
                {
                    auth_token = token,
                    delivery_needed = true,
                    merchant_id = merchantId,
                    amount_cents = amount,
                    currency = "EGP",
                    merchant_order_id = ordId,
                    shipping_data = new
                    {
                        apartment = model.Apartment,
                        email = model.Email,
                        floor = model.Floor,
                        first_name = model.FirstName,//user.Name.Substring(0,user.Name.IndexOf(" ")),
                        street = model.Address,
                        building = model.BuildingNumber,
                        phone_number = model.Mobile,
                        postal_code = model.PostalCode,
                        city="Cairo", //ToDo
                        country="Egypt",
                        last_name = model.LastName,//user.Name.Substring(user.Name.IndexOf(" ")),
                        state ="Cairo" //ToDo
                    }
                });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OrderResult>(data);
                    //await GetPaymentKey(token, result.Id, amount, integration_Id);
                    return result; ;
                }
            }
            return null;
        }


        private async Task<PKeyResult> GetPaymentKey(string token, long orderid, double amount)
        {
            string apiUrl = $"https://accept.paymobsolutions.com/api/acceptance/payment_keys";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var bodyJS = JsonConvert.SerializeObject(new
                {
                    auth_token = token,
                    amount_cents = amount,
                    expiration = 36000,
                    currency = "EGP",
                    order_id = orderid,
                    integration_id = AppSettings.PaymentIntegrationId,
                    billing_data=new
                    {
                        apartment= "803",
                        email = "claudette09@exa.com",
                        floor = "42",
                        first_name = "Clifford",
                        street = "Ethan Land",
                        building = "8028",
                        phone_number = "+86(8)9135210487",
                        shipping_method = "PKG",
                        postal_code = "01898",
                        city = "Jaskolskiburgh",
                        country = "CR",
                        last_name = "Nicolas",
                        state = "Utah",
                    }

                });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PKeyResult>(data);
                    return result;
                }
            }
            return null;
        }


        private async Task<BookSessionResult> BookSession(int AId,long order)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/LawyerApi/BookAppointment/{AId}/{order}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BookSessionResult>(data);
                    return result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
            }

            return null;
        }
        private async Task<bool> ConfirmBooking(long order)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/SessionApi/ConfirmPayment/{order}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<bool>(data);
                    return result;
                }
            }

            return false;
        }


        private async Task<AppointmentDTO> GetAppointmentDetails(int appointmentId)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/LawyerApi/GetAppointmentDetails/{appointmentId}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<AppointmentDTO>(data);
                    return result;
                }
            }
            return null;
        }

        string redirectUrl;
        string fToken;
        int order_id;

        #region Old PayAction
        //[HttpPost]
        //[Route("/Lawyer/PayAction/{lawyerId}/{AppointmentId}/{pType}")]
        //public async Task<IActionResult> PayAction(int lawyerId, int AppointmentId, int pType, int type_drp)
        //{
        //    var details = await GetAppointmentDetails(AppointmentId);
        //    //var details = await GetServiceDetails(serviceId, lawyerId);
        //    await GetToken(details.Amount * 100, pType == 1 ? AppSettings.PaymentIntegrationId2 : AppSettings.PaymentIntegrationIdCash2
        //        );
        //    ////var userName = Request.Cookies["UserName"];
        //    ////var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    ////var order = new Order
        //    ////{
        //    ////    merchant_order_id = order_id,
        //    ////    serviceId = serviceId,
        //    ////    lawyerId = lawyerId,
        //    ////    clientId = userId
        //    ////};
        //    var token = Request.Cookies["token"];
        //    //string apiUrl = $"http://{Request.Host}/api/ServiceApi/NewOrd/{serviceId}/{lawyerId}/{order_id}";
        //    string apiUrl = $"http://{Request.Host}/api/LawyerApi/BookAppointment/{AppointmentId}/{order_id}";
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(apiUrl);
        //        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        //        HttpResponseMessage response = await client.GetAsync(apiUrl);
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var data = await response.Content.ReadAsStringAsync();
        //            var result = JsonConvert.DeserializeObject<BookSessionResult>(data);

        //        }
        //        else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        //        {
        //            return null;
        //        }
        //    }
        //    ////Redirect("http://accept.paymobsolutions.com/api/acceptance/iframes/7127?payment_token=" + fToken);
        //    ////return View(new BaseViewModel(Request));
        //    ////return (Redirect("http://accept.paymobsolutions.com/api/acceptance/iframes/7127?payment_token=" + fToken));
        //    if (pType == 1)
        //    {
        //        return Ok(new { token = fToken, ptype = pType });
        //    }
        //    else
        //    {
        //        return Ok(new { redirect_url = redirectUrl, ptype = pType });
        //    }
        //}
        #endregion
            //
        //[HttpPost]
        [Route("/Lawyer/PayAction/{lawyerId}/{AppointmentId}")]
        public async Task<IActionResult> PayAction(int lawyerId, int AppointmentId,[FromForm] DeliveryDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var details = await GetAppointmentDetails(AppointmentId);
                if (details != null && details.CanReserved)
                {
                    var res = await GetToken(details.Amount * 100, AppSettings.PaymentIntegrationIdCash2);
                    var order = await OrderCreation(res.Token, res.Profile.Id, details.Amount * 100, AppSettings.PaymentIntegrationIdCash2,model);
                    var pkey = await GetPaymentKey(res.Token, order.Id, details.Amount * 100, AppSettings.PaymentIntegrationIdCash2);

                    if (!string.IsNullOrEmpty(pkey.Token))
                    {

                        var bookRes = await BookSession(AppointmentId, order.Id);
                        if (bookRes == null)
                            return RedirectToAction("Login", "Account");
                    }
                    return RedirectToAction("BookingSuccessful", new { txn_response_code = 0, success = true, order = order.Id, amount_cents=order.AmountCents });

                }
                //return Ok(new { token = fToken, ptype = 2 });
            }
            return null;
        }

        [Route("/Lawyer/PayWithCard/{AppointmentId}")]
        public async Task<IActionResult> PayWithCard(int AppointmentId)
        {
            var details = await GetAppointmentDetails(AppointmentId);
            if (details != null && details.CanReserved)
            {
                var res = await GetToken(1, 5521);
                var order = await OrderCreation(res.Token, res.Profile.Id, details.Amount * 100, 5521);
                var pkey = await GetPaymentKey(res.Token, order.Id, details.Amount * 100, 5521);

                if (!string.IsNullOrEmpty(pkey.Token))
                {
                    var bookRes = await BookSession(AppointmentId, order.Id);
                    if (bookRes == null)
                        return RedirectToAction("Login", "Account");
                    var iframeId = AppSettings.PaymentIframeId;
                    return Redirect($"http://accept.paymobsolutions.com/api/acceptance/iframes/{iframeId}?payment_token={pkey.Token}");
                }
            }
            return null;
        }

        private async Task<PaymentTokenResult> GetToken(double price, int integration_Id)
        {
            string apiUrl = "https://accept.paymobsolutions.com/api/auth/tokens";
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var paymentApiKey = AppSettings.PaymentApiKey;

                var bodyJS = JsonConvert.SerializeObject(new { api_key = paymentApiKey });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<PaymentTokenResult>(data);
                    //await OrderCreation(result.Token, AppSettings.MerchantId, price, integration_Id);
                    return result;
                }
            }
            return null;
        }

        private async Task<OrderResult> OrderCreation(string token, long merchantId, double amount
            , int integration_Id)
        {
            string apiUrl = $"https://accept.paymobsolutions.com/api/ecommerce/orders";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var ordId = Utilities.GetUniqueKeyNums(6);
                order_id = int.Parse(ordId);
                var bodyJS = JsonConvert.SerializeObject(new
                {
                    auth_token = token,
                    delivery_needed = false,
                    merchant_id = merchantId,
                    amount_cents = amount,
                    currency = "EGP",
                    //merchant_order_id = Guid.NewGuid().ToString()
                    merchant_order_id = ordId
                });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<OrderResult>(data);
                    await GetPaymentKey(token, result.Id, amount, integration_Id);
                    return result; ;
                }
            }
            return null;
        }

        private async Task<PKeyResult> GetPaymentKey(string token, long orderid, double amount, int integration_Id)
        {
            string apiUrl = $"https://accept.paymobsolutions.com/api/acceptance/payment_keys";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var bodyJS = JsonConvert.SerializeObject(new
                {
                    auth_token = token,
                    amount_cents = amount,
                    expiration = 36000,
                    currency = "EGP",
                    order_id = orderid.ToString(),
                    integration_id = integration_Id,
                    billing_data = new
                    {
                        apartment = "1",
                        email = "magamaleldin@gmail.com",
                        floor = "1",
                        first_name = "Mohamed",
                        street = "Showayefat",
                        building = "8028",
                        phone_number = "+201029006313",
                        shipping_method = "PKG",
                        postal_code = "01898",
                        city = "New Cairo",
                        country = "EG",
                        last_name = "Gamal",
                        state = "Cairo",
                    }

                });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<PKeyResult>(data);
                    fToken = result.Token;
                    if (integration_Id == AppSettings.PaymentIntegrationIdCash2)
                    {
                        await CashPayment(fToken);
                        return result;
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            return null;
        }

        private async Task<CashPayResult> CashPayment(string token)
        {
            string apiUrl = $"https://accept.paymobsolutions.com/api/acceptance/payments/pay";
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var bodyJS = JsonConvert.SerializeObject(new
                {
                    payment_token = token,
                    source = new
                    {
                        identifier = "cash",
                        subtype = "CASH"
                    }

                });
                var body = new StringContent(bodyJS, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(apiUrl, body);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<CashPayResult>(data);
                    redirectUrl = result.redirection_url;
                    return result;
                }
            }
            return null;
        }

        private async Task<BookSessionResult> PayService(int serviceId, int lawyerId, long order)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/ServiceApi/PayService/{serviceId}/{lawyerId}/{order}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<BookSessionResult>(data);
                    return result;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return null;
                }
            }

            return null;
        }
    }
}
