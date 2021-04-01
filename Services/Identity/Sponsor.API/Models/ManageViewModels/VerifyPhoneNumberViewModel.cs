using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models.ManageViewModels
{
    public record VerifyPhoneNumberViewModel
    {
        [Required]
        public string Code { get; init; }

        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; init; }
    }
}
