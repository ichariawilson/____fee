using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public class Location
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "County name is required")]
        [Column(TypeName = "nvarchar(50)")]
        public string County { get; set; }

        public List<Profile> Profiles { get; set; }

        public List<School> Schools { get; set; }
    }
}
