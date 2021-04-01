using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels
{
    public class TimelineViewModel
    {
        [Display(Name = "Timeline ID")]
        public string TimelineId { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public Period Period { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "School is required")]
        [Display(Name = "School")]
        public string SchoolId { get; set; }
        public IEnumerable<SelectListItem> Schools { get; set; }
    }
}
