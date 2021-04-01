using System.Collections.Generic;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Models
{
    public class UpdateBasketRequest
    {
        public string StudentId { get; set; }

        public IEnumerable<UpdateBasketRequestItemData> Items { get; set; }
    }

}
