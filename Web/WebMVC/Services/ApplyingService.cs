using Microsoft.Fee.WebMVC.ViewModels;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Infrastructure;
using WebMVC.Services.ModelDTOs;

namespace Microsoft.Fee.WebMVC.Services
{
    public class ApplyingService : IApplyingService
    {
        private HttpClient _httpClient;
        private readonly string _remoteServiceBaseUrl;
        private readonly IOptions<AppSettings> _settings;


        public ApplyingService(HttpClient httpClient, IOptions<AppSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;

            _remoteServiceBaseUrl = $"{settings.Value.ApplyUrl}/a/api/v1/applications";
        }

        async public Task<Application> GetApplication(ApplicationUser user, string id)
        {
            var uri = API.Application.GetApplication(_remoteServiceBaseUrl, id);

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<Application>(responseString);

            return response;
        }

        async public Task<List<Application>> GetMyApplications(ApplicationUser user)
        {
            var uri = API.Application.GetAllMyApplications(_remoteServiceBaseUrl);

            var responseString = await _httpClient.GetStringAsync(uri);

            var response = JsonConvert.DeserializeObject<List<Application>>(responseString);

            return response;
        }

        async public Task CancelApplication(string applicationId)
        {
            var application = new ApplicationDTO()
            {
                ApplicationNumber = applicationId
            };

            var uri = API.Application.CancelApplication(_remoteServiceBaseUrl);
            var applicationContent = new StringContent(JsonConvert.SerializeObject(application), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, applicationContent);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error cancelling application, try later.");
            }

            response.EnsureSuccessStatusCode();
        }

        async public Task GrantApplication(string applicationId)
        {
            var application = new ApplicationDTO()
            {
                ApplicationNumber = applicationId
            };

            var uri = API.Application.GrantApplication(_remoteServiceBaseUrl);
            var applicationContent = new StringContent(JsonConvert.SerializeObject(application), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(uri, applicationContent);

            if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                throw new Exception("Error in grant application process, try later.");
            }

            response.EnsureSuccessStatusCode();
        }

        public void OverrideUserInfoIntoApplication(Application original, Application destination)
        {
            destination.DateofBirth = original.DateofBirth;
            destination.IDNumber = original.IDNumber;
            destination.Request = original.Request;
        }

        public Application MapUserInfoIntoApplication(ApplicationUser user, Application application)
        {
            application.DateofBirth = user.DateofBirth;
            application.IDNumber = user.IDNumber;
            application.Request = user.Request;

            return application;
        }

        public BasketDTO MapApplicationToBasket(Application application)
        {
            return new BasketDTO()
            {
                DateofBirth = application.DateofBirth,
                IDNumber = application.IDNumber,
                Request = application.Request,
                GenderId = 1,
                HobbyId = 1,
                LocationId = 1,
                SchoolId = 1,
                PaymentTypeId = 1,
                Student = application.Student,
                RequestId = application.RequestId
            };
        }
    }
}
