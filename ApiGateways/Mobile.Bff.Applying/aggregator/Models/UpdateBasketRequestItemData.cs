namespace Microsoft.Fee.Mobile.Applying.HttpAggregator.Models
{

    public class UpdateBasketRequestItemData
    {
        public string Id { get; set; }          // Basket id

        public int ScholarshipItemId { get; set; }      // Scholarship Item id

        public int Slots { get; set; }       // Slots
    }

}
