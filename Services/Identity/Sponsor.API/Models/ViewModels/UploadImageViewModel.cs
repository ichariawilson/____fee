using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models.ViewModels
{
    public class UploadImageViewModel
    {
        [Required]
        [Display(Name = "Profile Picture")]
        public IFormFile ProfilePicture { get; set; }
    }
}
