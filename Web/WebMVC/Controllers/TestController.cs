using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.WebMVC.Services;
using Microsoft.Fee.WebMVC.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebMVC.Controllers
{
    class TestPayload
    {
        public int ScholarshipItemId { get; set; }

        public string BasketId { get; set; }

        public int Slots { get; set; }
    }

    [Authorize]
    public class TestController : Controller
    {
        private readonly IHttpClientFactory _client;
        private readonly IIdentityParser<ApplicationUser> _appUserParser;

        public TestController(IHttpClientFactory client, IIdentityParser<ApplicationUser> identityParser)
        {
            _client = client;
            _appUserParser = identityParser;
        }

        public async Task<IActionResult> Ocelot()
        {
            var url = "http://apigw/applying/api/v1/basket/items";

            var payload = new TestPayload()
            {
                ScholarshipItemId = 1,
                Slots = 1,
                BasketId = _appUserParser.Parse(User).Id
            };

            var content = new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");


            var response = await _client.CreateClient(nameof(IBasketService))
                .PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();

                return Ok(str);
            }
            else
            {
                return Ok(new { response.StatusCode, response.ReasonPhrase });
            }
        }
    }
}
