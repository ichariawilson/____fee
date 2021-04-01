namespace Microsoft.Fee.WebMVC.ViewModels
{
    public record ApplicationItem
    {
        public int ScholarshipItemId { get; init; }

        public string ScholarshipItemName { get; init; }

        public decimal SlotAmount { get; init; }

        public int Slots { get; init; }

        public string PictureUrl { get; init; }
    }
}
