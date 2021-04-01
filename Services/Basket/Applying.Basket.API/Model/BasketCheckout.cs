using System;

namespace Applying.Basket.API.Model
{
    public class BasketCheckout
    {
        public string DateofBirth { get; set; }
        public string IDNumber { get; set; }
        public decimal Request { get; set; }
        public int GenderId { get; set; }
        public int HobbyId { get; set; }
        public int LocationId { get; set; }
        public int SchoolId { get; set; }
        public int PaymentTypeId { get; set; }
        public string Student { get; set; }
        public Guid RequestId { get; set; }
    }
}

