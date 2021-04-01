using IdentityServer4.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public record ErrorViewModel
    {
        public ErrorMessage Error { get; set; }
    }
}
