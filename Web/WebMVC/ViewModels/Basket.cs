using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.WebMVC.ViewModels
{
    public record Basket
    {
        // Use property initializer syntax.
        // While this is often more useful for read only 
        // auto implemented properties, it can simplify logic
        // for read/write properties.
        public List<BasketItem> Items { get; init; } = new List<BasketItem>();
        public string StudentId { get; init; }

        public decimal Total()
        {
            return Math.Round(Items.Sum(x => x.SlotAmount * x.Slots), 2);
        }
    }
}
