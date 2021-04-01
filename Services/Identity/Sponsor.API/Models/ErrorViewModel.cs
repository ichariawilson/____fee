using IdentityServer4.Models;

namespace Microsoft.Fee.Services.Sponsor.API.Models
{
    public record ErrorViewModel
    {
        public ErrorMessage Error { get; set; }
    }
}
