namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Models
{
    public class BasketDataItem
    {
        public string Id { get; set; }

        public int ScholarshipItemId { get; set; }

        public string ScholarshipItemName { get; set; }

        public decimal SlotAmount { get; set; }

        public decimal OldSlotAmount { get; set; }

        public int Slots { get; set; }

        public string PictureUrl { get; set; }
    }
}
