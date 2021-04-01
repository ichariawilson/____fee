using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Fee.WebMVC.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Fee.WebMVC.Services
{
    public interface IScholarshipService
    {
        Task<Scholarship> GetScholarshipItems(int page, int take, int? location, int? educationlevel);
        Task<IEnumerable<SelectListItem>> GetCurrencies();
        Task<IEnumerable<SelectListItem>> GetDurations();
        Task<IEnumerable<SelectListItem>> GetEducationLevels();
        Task<IEnumerable<SelectListItem>> GetFeeStructures();
        Task<IEnumerable<SelectListItem>> GetInterests();
        Task<IEnumerable<SelectListItem>> GetLocations();
    }
}
