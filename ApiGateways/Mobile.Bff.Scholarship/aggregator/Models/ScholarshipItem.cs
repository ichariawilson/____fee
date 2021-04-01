namespace Microsoft.Fee.Mobile.Scholarship.HttpAggregator.Models
{
    public class ScholarshipItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string PictureUri { get; set; }
    }
}
