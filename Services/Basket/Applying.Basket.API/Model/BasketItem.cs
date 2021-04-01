using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Applying.Basket.API.Model
{
    public class BasketItem : IValidatableObject
    {
        public string Id { get; set; }
        public int ScholarshipItemId { get; set; }
        public string ScholarshipItemName { get; set; }
        public decimal SlotAmount { get; set; }
        public decimal OldSlotAmount { get; set; }
        public int Slots { get; set; }
        public string PictureUrl { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (Slots < 1)
            {
                results.Add(new ValidationResult("Invalid number of slots", new[] { "Slots" }));
            }

            return results;
        }
    }
}
