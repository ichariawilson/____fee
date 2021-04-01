namespace Applying.API.Application.Models
{
    public class BasketItem
    {
        public string Id { get; init; }
        public int ScholarshipItemId { get; init; }
        public string ScholarshipItemName { get; init; }
        public decimal SlotAmount { get; init; }
        public decimal OldSlotAmount { get; init; }
        public int Slots { get; init; }
        public string PictureUrl { get; init; }
    }
}
