using System.Collections.Generic;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Models
{
    public class UpdateBasketItemsRequest
    {
        public string BasketId { get; set; }

        public ICollection<UpdateBasketItemData> Updates { get; set; }

        public UpdateBasketItemsRequest()
        {
            Updates = new List<UpdateBasketItemData>();
        }
    }

}
