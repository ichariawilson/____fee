using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.WebMVC.ViewModels
{
    // Profile data for application users
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string DateofBirth { get; set; }
        [Required]
        public string IDNumber { get; set; }
        [Required]
        public decimal Request { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        public int Hobby { get; set; }
        [Required]
        public int Location { get; set; }
        [Required]
        public int School { get; set; }
        [Required]
        public int PaymentType { get; set; }
    }
}
