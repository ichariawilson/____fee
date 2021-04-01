using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models
{
    public class Location
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string County { get; set; }

        public List<Profile> Profiles { get; set; }

        public List<School> Schools { get; set; }
    }
}
