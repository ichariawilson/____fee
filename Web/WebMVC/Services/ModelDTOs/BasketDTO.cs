using System;
using System.ComponentModel.DataAnnotations;

namespace WebMVC.Services.ModelDTOs
{
    public record BasketDTO
    {
        [Required]
        public string DateofBirth { get; init; }
        [Required]
        public string IDNumber { get; init; }
        [Required]
        public decimal Request { get; init; }

        public int GenderId { get; init; }

        public int HobbyId { get; init; }

        public int LocationId { get; init; }

        public int SchoolId { get; init; }

        public int PaymentTypeId { get; init; }

        public string Student { get; init; }

        [Required]
        public Guid RequestId { get; init; }
    }
}

