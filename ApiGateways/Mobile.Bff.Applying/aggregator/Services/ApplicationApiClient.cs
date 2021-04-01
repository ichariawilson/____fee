using Microsoft.Fee.Mobile.Applying.HttpAggregator.Config;
using Microsoft.Fee.Mobile.Applying.HttpAggregator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Services
{
    public class ApplicationApiClient : IApplicationApiClient
    {
        private readonly HttpClient _apiClient;
        private readonly ILogger<ApplicationApiClient> _logger;
        private readonly UrlsConfig _urls;

        public ApplicationApiClient(HttpClient httpClient, ILogger<ApplicationApiClient> logger, IOptions<UrlsConfig> config)
        {
            _apiClient = httpClient;
            _logger = logger;
            _urls = config.Value;
        }

        public async Task<ApplicationData> GetApplicationDraftFromBasketAsync(BasketData basket)
        {
            var uri = _urls.Applications + UrlsConfig.ApplicationsOperations.GetApplicationDraft();
            var content = new StringContent(JsonConvert.SerializeObject(basket), System.Text.Encoding.UTF8, "application/json");
            var response = await _apiClient.PostAsync(uri, content);

            response.EnsureSuccessStatusCode();

            var applicationsDraftResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ApplicationData>(applicationsDraftResponse);
        }
    }
}
