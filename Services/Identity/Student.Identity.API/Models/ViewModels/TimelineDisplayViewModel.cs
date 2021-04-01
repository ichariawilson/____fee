using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels
{
    public class TimelineDisplayViewModel
    {
        [Display(Name = "Timeline Id")]
        public Guid TimelineId { get; set; }

        public string Description { get; set; }

        public Period Period { get; set; }

        public string School { get; set; }
    }
}
