namespace Microsoft.Fee.WebMVC.ViewModels
{
    public record BasketItem
    {
        public string Id { get; init; }
        public string ScholarshipItemId { get; init; }
        public string ScholarshipItemName { get; init; }
        public decimal SlotAmount { get; init; }
        public decimal OldSlotAmount { get; init; }
        public int Slots { get; init; }
        public string PictureUrl { get; init; }
    }
}
