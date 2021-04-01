using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Scholarship.API.ViewModel
{
    public class UploadImageViewModel
    {
        [Required]
        [Display(Name = "Profile Picture")]
        public IFormFile PictureUri { get; set; }
    }
}
