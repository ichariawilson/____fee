using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Sponsor.API.Models
{
    public class Profile
    {
        [Key]
        public Guid Id { get; set; }

        //[Required]
        //[Display(Name = "Profile Picture")]
        //public string ProfilePicture { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateofBirth { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        public string EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string LocationId { get; set; }
        public Location Location { get; set; }

        [Required(ErrorMessage = "School is required")]
        public string SchoolId { get; set; }
        public School School { get; set; }

        //public string UserId { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}
