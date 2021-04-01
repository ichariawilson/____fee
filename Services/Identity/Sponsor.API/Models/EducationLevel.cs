using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models
{
    public class EducationLevel
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Education Level is required")]
        public string Level { get; set; }

        public List<Profile> Profiles { get; set; }

        public List<School> Schools { get; set; }
    }
}
