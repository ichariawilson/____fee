using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
