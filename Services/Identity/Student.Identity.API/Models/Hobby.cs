using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public class Hobby
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Hobby name is required")]
        [Column(TypeName = "nvarchar(50)")]
        [Display(Name = "Hobby")]
        public string Name { get; set; }

        public List<Profile> Profiles { get; set; }
    }
}
