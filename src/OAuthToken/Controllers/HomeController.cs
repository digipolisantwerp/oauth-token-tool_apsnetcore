using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAuthToken.Models;

namespace OAuthToken.Controllers
{
    public class HomeController : Controller
    {
        private static Dictionary<string, AuthenticatePost> tempCache = new Dictionary<string, AuthenticatePost>();

        public IActionResult Index()
        {
            return View(new AuthenticatePost());
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticatePost model)
        {
            var sb = new StringBuilder();
            sb.Append(model.BaseUrl)
                .Append("?lng=en")
                .Append("&response_type=code")
                .Append("&client_id=").Append(model.ClientId)
                .Append("&redirect_uri=").Append(model.CallbackUri)
                .Append("&service=").Append(model.Service)
                .Append("&scope=").Append(model.Scope)
                .Append("&state=").Append(HttpContext.TraceIdentifier);

            tempCache.Add(HttpContext.TraceIdentifier, model);

            return Redirect(sb.ToString());
        }

        public async Task<IActionResult> Callback([FromQuery] CallbackQuery queryParams)
        {
            var settings = tempCache[queryParams.State];
            tempCache.Remove(queryParams.State);

            var postData = new Dictionary<string, string>();
            postData.Add("client_id", settings.ClientId);
            postData.Add("client_secret", settings.ClientSecret);
            postData.Add("code", queryParams.Code);
            postData.Add("grant_type", "authorization_code");

            using (var client = new HttpClient())
            using (var requestContent = new FormUrlEncodedContent(postData))
            {
                client.BaseAddress = new Uri(settings.TokenUrl);

                var response = await client.PostAsync("", requestContent);

                var payload = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<TokenResponse>(payload);

                return View(data);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ConvertToken(TokenResponse tokenData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api-gw-a.antwerpen.be/jimmyhannon/test/v1/api/requestinfo");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenData.Access_token);
                client.DefaultRequestHeaders.Add("apikey", "9d158a37-79a2-46de-941b-d009b555d017");

                var response = await client.GetAsync("");

                var payload = await response.Content.ReadAsStringAsync();

                var data = JsonConvert.DeserializeObject<RequestInfoResponse>(payload);

                ViewBag.oauthToken = tokenData.Access_token;
                ViewBag.jwt = data.HttpContext_Request_Headers.Single(x => x.Key == "Authorization")
                                      .Value.SingleOrDefault().Replace("Bearer ", "");

                return View();
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
