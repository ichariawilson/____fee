using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models.ViewModels
{
    public class LocationViewModel
    {
        [Display(Name = "Location Id")]
        public string LocationId { get; set; }

        [Required(ErrorMessage = "County name is required")]
        [Display(Name = "County")]
        public string County { get; set; }
    }
}
