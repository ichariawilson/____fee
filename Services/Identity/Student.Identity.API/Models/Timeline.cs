using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public class Timeline
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Period is required")]
        public Period Period { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }

        //[Required]
        //[Display(Name = "Qualification")]
        //public string File { get; set; }

        [Required(ErrorMessage = "School is required")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        //public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}
