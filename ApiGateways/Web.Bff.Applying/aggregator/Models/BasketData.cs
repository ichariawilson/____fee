using System.Collections.Generic;

namespace Microsoft.Fee.Web.Applying.HttpAggregator.Models
{
    public class BasketData
    {
        public string StudentId { get; set; }

        public List<BasketDataItem> Items { get; set; } = new List<BasketDataItem>();

        public BasketData()
        {
        }

        public BasketData(string studentId)
        {
            StudentId = studentId;
        }
    }
}
