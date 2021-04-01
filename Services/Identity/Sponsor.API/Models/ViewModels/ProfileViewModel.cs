using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Profile ID")]
        public string ProfileId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        [Display(Name = "Education Level")]
        public string EducationLevelId { get; set; }
        public IEnumerable<SelectListItem> EducationLevels { get; set; }

        [Required(ErrorMessage = "County is required")]
        [Display(Name = "Location")]
        public string LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }

        [Required(ErrorMessage = "School is required")]
        [Display(Name = "School")]
        public string SchoolId { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; }
    }
}
