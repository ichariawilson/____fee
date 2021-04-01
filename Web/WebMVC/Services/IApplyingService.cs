using Microsoft.Fee.WebMVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebMVC.Services.ModelDTOs;

namespace Microsoft.Fee.WebMVC.Services
{
    public interface IApplyingService
    {
        Task<List<Application>> GetMyApplications(ApplicationUser user);
        Task<Application> GetApplication(ApplicationUser user, string applicationId);
        Task CancelApplication(string applicationId);
        Task GrantApplication(string applicationId);
        Application MapUserInfoIntoApplication(ApplicationUser user, Application application);
        BasketDTO MapApplicationToBasket(Application application);
        void OverrideUserInfoIntoApplication(Application original, Application destination);
    }
}
