using System.ComponentModel.DataAnnotations;

namespace WebMVC.Services.ModelDTOs
{
    public record ApplicationDTO
    {
        [Required]
        public string ApplicationNumber { get; init; }
    }
}