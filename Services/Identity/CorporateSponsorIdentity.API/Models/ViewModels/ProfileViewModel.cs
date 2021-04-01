using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Display(Name = "Profile ID")]
        public string ProfileId { get; set; }

        [Required(ErrorMessage = "Website link is required")]
        [Display(Name = "Web URL")]
        public string WebURL { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        [Display(Name = "Education Level")]
        public string EducationLevelId { get; set; }
        public IEnumerable<SelectListItem> EducationLevels { get; set; }

        [Required(ErrorMessage = "County is required")]
        [Display(Name = "Location")]
        public string LocationId { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
    }
}
