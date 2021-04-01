using System.Collections.Generic;

namespace Microsoft.Fee.Services.Applying.Basket.API.Model
{
    public class StudentBasket
    {
        public string StudentId { get; set; }

        public List<BasketItem> Items { get; set; } = new List<BasketItem>();

        public StudentBasket()
        {
        }

        public StudentBasket(string studentId)
        {
            StudentId = studentId;
        }
    }
}
