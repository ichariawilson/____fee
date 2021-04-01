using System.Collections.Generic;

namespace Applying.API.Application.Models
{
    public class StudentBasket
    {
        public string StudentId { get; set; }
        public List<BasketItem> Items { get; set; }

        public StudentBasket(string studentId)
        {
            StudentId = studentId;
            Items = new List<BasketItem>();
        }
    }
}
