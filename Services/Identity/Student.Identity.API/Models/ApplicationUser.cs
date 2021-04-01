using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    // Profile data for application students
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "First Name is required")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Middle Name is required")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Middle Name")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "ID Number is required")]
        [Column(TypeName = "nvarchar(8)")]
        [Display(Name = "ID Number")]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Request Amount is required")]
        [Column(TypeName = "decimal(18,4)")]
        [Range(500, 100000)]
        [Display(Name = "Request Amount")]
        public decimal Request { get; set; }

        //public virtual Profile Profile { get; set; }
        //public virtual Timeline Timeline { get; set; }
    }
}
