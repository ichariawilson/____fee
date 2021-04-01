using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models
{
    public class School
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "School name is required")]
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
