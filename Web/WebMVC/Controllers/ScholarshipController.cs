using Microsoft.AspNetCore.Mvc;
using Microsoft.Fee.WebMVC.Services;
using Microsoft.Fee.WebMVC.ViewModels.ScholarshipViewModels;
using Microsoft.Fee.WebMVC.ViewModels.Pagination;
using System;
using System.Threading.Tasks;

namespace Microsoft.Fee.WebMVC.Controllers
{
    public class ScholarshipController : Controller
    {
        private IScholarshipService _scholarshipSvc;

        public ScholarshipController(IScholarshipService scholarshipSvc) =>
            _scholarshipSvc = scholarshipSvc;

        public async Task<IActionResult> Index(int? LocationFilterApplied, int? EducationLevelFilterApplied, int? page, [FromQuery] string errorMsg)
        {
            var itemsPage = 10;
            var scholarship = await _scholarshipSvc.GetScholarshipItems(page ?? 0, itemsPage, LocationFilterApplied, EducationLevelFilterApplied);
            var vm = new IndexViewModel()
            {
                ScholarshipItems = scholarship.Data,
                Locations = await _scholarshipSvc.GetLocations(),
                EducationLevels = await _scholarshipSvc.GetEducationLevels(),
                LocationFilterApplied = LocationFilterApplied ?? 0,
                EducationLevelFilterApplied = EducationLevelFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page ?? 0,
                    ItemsPerPage = scholarship.Data.Count,
                    TotalItems = scholarship.Count,
                    TotalPages = (int)Math.Ceiling(((decimal)scholarship.Count / itemsPage))
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            ViewBag.BasketInoperativeMsg = errorMsg;

            return View(vm);
        }
    }
}