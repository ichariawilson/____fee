using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Fee.WebMVC.ViewModels.Pagination;
using System.Collections.Generic;

namespace Microsoft.Fee.WebMVC.ViewModels.ScholarshipViewModels
{
    public class IndexViewModel
    {
        public IEnumerable<ScholarshipItem> ScholarshipItems { get; set; }
        public IEnumerable<SelectListItem> Currencies { get; set; }
        public IEnumerable<SelectListItem> Durations { get; set; }
        public IEnumerable<SelectListItem> EducationLevels { get; set; }
        public IEnumerable<SelectListItem> FeeStructures { get; set; }
        public IEnumerable<SelectListItem> Interests { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public int? CurrencyFilterApplied { get; set; }
        public int? DurationFilterApplied { get; set; }
        public int? EducationLevelFilterApplied { get; set; }
        public int? FeeStructureFilterApplied { get; set; }
        public int? InterestFilterApplied { get; set; }
        public int? LocationFilterApplied { get; set; }
        public PaginationInfo PaginationInfo { get; set; }
    }
}
