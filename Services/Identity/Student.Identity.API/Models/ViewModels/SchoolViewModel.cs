using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels
{
    public class SchoolViewModel
    {
        [Display(Name = "School Id")]
        public int SchoolId { get; set; }

        [Required(ErrorMessage = "School name is required")]
        [Display(Name = "School")]
        public string Name { get; set; }

        public string Level { get; set; }

        public string County { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        [Display(Name = "Education Level")]
        public string EducationLevelId { get; set; }
        public IEnumerable<SelectListItem> EducationLevels { get; set; }


        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
    }
}
