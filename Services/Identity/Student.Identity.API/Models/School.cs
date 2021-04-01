using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public class School
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "School name is required")]
        [Column(TypeName = "nvarchar(100)")]
        [Display(Name = "School")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        public string EducationLevelId { get; set; }
        public EducationLevel EducationLevel { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string LocationId { get; set; }
        public Location Location { get; set; }
    }
}
