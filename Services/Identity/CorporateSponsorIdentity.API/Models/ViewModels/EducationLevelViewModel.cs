using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels
{
    public class EducationLevelViewModel
    {
        [Display(Name = "Education Level Id")]
        public string EducationLevelId { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        [Display(Name = "Education Level")]
        public string Level { get; set; }
    }
}
