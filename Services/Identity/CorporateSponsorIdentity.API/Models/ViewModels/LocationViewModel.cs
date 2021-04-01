using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels
{
    public class LocationViewModel
    {
        [Display(Name = "Location Id")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "County")]
        public string County { get; set; }
    }
}
