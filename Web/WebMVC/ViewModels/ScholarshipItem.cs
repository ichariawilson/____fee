namespace Microsoft.Fee.WebMVC.ViewModels
{
    public record ScholarshipItem
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Amount { get; init; }
        public string PictureUri { get; init; }
        public int ScholarshipCurrencyId { get; init; }
        public string ScholarshipCurrency { get; init; }
        public int ScholarshipDurationId { get; init; }
        public string ScholarshipDuration { get; init; }
        public int ScholarshipEducationLevelId { get; init; }
        public string ScholarshipEducationLevel { get; init; }
        public int ScholarshipInterestId { get; init; }
        public string ScholarshipInterest { get; init; }
        public int ScholarshipLocationId { get; init; }
        public string ScholarshipLocation { get; init; }
    }
}