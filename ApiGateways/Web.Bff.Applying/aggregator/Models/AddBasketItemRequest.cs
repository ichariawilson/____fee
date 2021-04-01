namespace Microsoft.Fee.Web.Applying.HttpAggregator.Models
{
    public class AddBasketItemRequest
    {
        public int ScholarshipItemId { get; set; }

        public string BasketId { get; set; }

        public int Slots { get; set; }

        public AddBasketItemRequest()
        {
            Slots = 1;
        }
    }
}
