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

namespace ElMaitre.Web3.Controllers
{
    public class ServiceController : BaseController
    {
        private AppSettings AppSettings { get; set; }
        ApplicationDbContext con;
        public ServiceController(IOptions<AppSettings> settings, ApplicationDbContext con)
        {
            AppSettings = settings.Value;
            this.con = con;
        }

        [Route("/Service/Index")]
        public async Task<IActionResult> Index(string hmac, int merchant_order_id, 
            string created_at, string currency, bool is_refund, int txn_response_code, int profile_id, 
            bool success, int order, int id, int amount_cents)
        {
            if (hmac != null)
            {
                var token = Request.Cookies["token"];
                string apiUrl = $"http://{Request.Host}/api/ServiceApi/UpdateOrd/{merchant_order_id}/{hmac}/{created_at}/{currency}/{is_refund}/{txn_response_code}/{profile_id}/{success}/{order}/{id}/{amount_cents}";

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

            //return View(new BaseViewModel(Request));
            //return View(con.Orders.FirstOrDefault(o => o.merchant_order_id == merchant_order_id).lawyer);
            var bm = new BaseViewModel(Request);
            bm.Lawyer = con.Orders.Include("lawyer").Include("lawyer.User").FirstOrDefault(o => o.merchant_order_id == merchant_order_id).lawyer;
            return View(bm);
        }

        [Route("/Service/Index/{serviceId}/{lawyerId}")]
        public IActionResult Index(int serviceId, int lawyerId)
        {
            //return View(new BaseViewModel(Request));
            return View(new BaseViewModel(Request));
        }

        string redirectUrl;
        string fToken;
        int order_id;
        [HttpPost]
        [Route("/Service/PayAction/{serviceId}/{lawyerId}/{pType}")]
        public async Task<IActionResult> PayAction(int serviceId, int lawyerId, int pType, int type_drp)
        {
            var res = await GetToken(type_drp * 100, pType == 1 ? AppSettings.PaymentIntegrationId : AppSettings.PaymentIntegrationIdCash);
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/ServiceApi/NewOrd/{serviceId}/{lawyerId}/{order_id}";
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
            //Redirect("http://accept.paymobsolutions.com/api/acceptance/iframes/7127?payment_token=" + fToken);
            //return View(new BaseViewModel(Request));
            //return (Redirect("http://accept.paymobsolutions.com/api/acceptance/iframes/7127?payment_token=" + fToken));
            if (pType == 1)
            {
                return Ok(new { token = fToken, ptype = pType });
            }
            else
            {
                return Ok(new { redirect_url = redirectUrl, ptype = pType });
            }
        }

        [Route("/Service/PayWithCard/{serviceId}/{lawyerId}")]
        public async Task<IActionResult> PayWithCard(int serviceId, int lawyerId)
        {
            var details = await GetServiceDetails(serviceId, lawyerId);
            if (details != null && details.CanReserved)
            {
                var res = await GetToken(1, 4255);
                var order = await OrderCreation(res.Token, res.Profile.Id, details.Price * 100, 4255);
                var pkey = await GetPaymentKey(res.Token, order.Id, details.Price * 100, 4255);

                if (!string.IsNullOrEmpty(pkey.Token))
                {
                    var bookRes = await PayService(serviceId, lawyerId, order.Id);
                    if (bookRes == null)
                        return RedirectToAction("Login", "Account");


                    var iframeId = AppSettings.PaymentIframeId;
                    return Redirect($"http://accept.paymobsolutions.com/api/acceptance/iframes/{iframeId}?payment_token={pkey.Token}");
                }
            }
            return null;
        }

        private async Task<ServiceDTO> GetServiceDetails(int serviceId,int lawyerId)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/ServiceApi/Get/{serviceId}/{lawyerId}";

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ServiceDTO>(data);
                    return result;
                }
            }
            return null;
        }

        private async Task<PaymentTokenResult> GetToken(double price, int integration_Id)
        {
            string apiUrl = "http://accept.paymobsolutions.com/api/auth/tokens";
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
                    await OrderCreation(result.Token, AppSettings.MerchantId, price, integration_Id);
                    return result;
                }
            }
            return null;
        }

        private async Task<OrderResult> OrderCreation(string token, long merchantId, double amount
            , int integration_Id)
        {
            string apiUrl = $"http://accept.paymobsolutions.com/api/ecommerce/orders";
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
                    return result;;
                }
            }
            return null;
        }

        private async Task<PKeyResult> GetPaymentKey(string token, long orderid, double amount, int integration_Id)
        {
            string apiUrl = $"http://accept.paymobsolutions.com/api/acceptance/payment_keys";
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
                    if (integration_Id == AppSettings.PaymentIntegrationIdCash)
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
            string apiUrl = $"http://accept.paymobsolutions.com/api/acceptance/payments/pay";
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

        private async Task<bool> ConfirmBooking(long order)
        {
            var token = Request.Cookies["token"];
            string apiUrl = $"http://{Request.Host}/api/ServiceApi/ConfirmPayment/{order}";

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
    }
}
