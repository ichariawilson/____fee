using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using Applying.Domain.Exceptions;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate
{
    public class ApplicationItem : Entity
    {
        private string _scholarshipItemName;
        private string _pictureUrl;
        private decimal _slotAmount;
        private int _slots;

        public int ScholarshipItemId { get; private set; }

        protected ApplicationItem() { }

        public ApplicationItem(int scholarshipItemId, string scholarshipItemName, decimal slotAmount, string PictureUrl, int slots = 1)
        {
            if (slots <= 0)
            {
                throw new ApplyingDomainException("Invalid number of slots");
            }

            ScholarshipItemId = scholarshipItemId;

            _scholarshipItemName = scholarshipItemName;
            _slotAmount = slotAmount;
            _slots = slots;
            _pictureUrl = PictureUrl;
        }

        public string GetPictureUri() => _pictureUrl;

        public int GetSlots()
        {
            return _slots;
        }

        public decimal GetSlotAmount()
        {
            return _slotAmount;
        }

        public string GetApplicationItemScholarshipItemName() => _scholarshipItemName;

        public void AddSlots(int slots)
        {
            if (slots < 0)
            {
                throw new ApplyingDomainException("Invalid slots");
            }

            _slots += slots;
        }
    }
}
