using System;
using System.Collections.Generic;

namespace Microsoft.Fee.Mobile.Scholarship.HttpAggregator.Models
{
    public class ApplicationData
    {
        public string ApplicationNumber { get; set; }

        public DateTime Date { get; set; }

        public string Status { get; set; }

        public decimal Total { get; set; }

        public string Description { get; set; }

        public bool IsDraft { get; set; }

        public string Student { get; set; }

        public List<ApplicationItemData> ApplicationItems { get; } = new List<ApplicationItemData>();
    }

}
