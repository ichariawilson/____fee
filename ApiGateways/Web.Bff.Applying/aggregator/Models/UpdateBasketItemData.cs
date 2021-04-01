namespace Microsoft.Fee.Web.Applying.HttpAggregator.Models
{

    public class UpdateBasketItemData
    {
        public string BasketItemId { get; set; }

        public int NewSlts { get; set; }

        public UpdateBasketItemData()
        {
            NewSlts = 0;
        }
    }

}
