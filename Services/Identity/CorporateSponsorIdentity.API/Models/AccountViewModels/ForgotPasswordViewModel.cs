using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.AccountViewModels
{
    public record ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}
