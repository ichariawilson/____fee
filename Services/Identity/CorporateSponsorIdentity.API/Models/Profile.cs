using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models
{
    public class Profile
    {
        public Guid Id { get; set; }

        //[Required]
        //[Display(Name = "Profile Picture")]
        //public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "Website link is required")]
        [Display(Name = "Web URL")]
        public string WebURL { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        [Display(Name = "Education Level")]
        public string EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }

        [Required(ErrorMessage = "Countyl is required")]
        [Display(Name = "County")]
        public string LocationId { get; set; }
        public Location Location { get; set; }
    }
}
