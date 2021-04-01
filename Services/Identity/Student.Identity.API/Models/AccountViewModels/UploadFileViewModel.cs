using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models.AccountViewModels
{
    public class UploadFileViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile TimelineFile { get; set; }
    }
}
