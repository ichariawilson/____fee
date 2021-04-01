using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Fee.WebMVC.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Infrastructure;

namespace Microsoft.Fee.WebMVC.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ScholarshipService> _logger;

        private readonly string _remoteServiceBaseUrl;

        public ScholarshipService(HttpClient httpClient, ILogger<ScholarshipService> logger, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;

            _remoteServiceBaseUrl = $"{_settings.Value.ApplyUrl}/s/api/v1/scholarship/";
        }

        public async Task<Scholarship> GetScholarshipItems(int page, int take, int? location, int? educationlevel)
        {
            var uri = API.Scholarship.GetAllScholarshipItems(_remoteServiceBaseUrl, page, take, location, educationlevel);

            var responseString = await _httpClient.GetStringAsync(uri);

            var scholarship = JsonConvert.DeserializeObject<Scholarship>(responseString);

            return scholarship;
        }

        public async Task<IEnumerable<SelectListItem>> GetCurrencies()
        {
            var uri = API.Scholarship.GetAllCurrencies(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);
            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("currency")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetDurations()
        {
            var uri = API.Scholarship.GetAllDurations(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);
            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("duration")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetEducationLevels()
        {
            var uri = API.Scholarship.GetAllEducationLevels(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();

            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);

            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("educationlevel")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetFeeStructures()
        {
            var uri = API.Scholarship.GetAllFeeStructures(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);
            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("feestructure")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetInterests()
        {
            var uri = API.Scholarship.GetAllInterests(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);
            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("interest")
                });
            }

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetLocations()
        {
            var uri = API.Scholarship.GetAllLocations(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = null, Text = "All", Selected = true });

            var edlevels = JArray.Parse(responseString);
            foreach (var edlevel in edlevels.Children<JObject>())
            {
                items.Add(new SelectListItem()
                {
                    Value = edlevel.Value<string>("id"),
                    Text = edlevel.Value<string>("location")
                });
            }

            return items;
        }
    }
}
