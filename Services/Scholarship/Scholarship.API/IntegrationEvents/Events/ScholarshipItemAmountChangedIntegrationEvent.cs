namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events
{
    using BuildingBlocks.EventBus.Events;

    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be past tense
    // An Integration Event is an event that can cause side effects to other microservices, Bounded-Contexts or external systems.
    public record ScholarshipItemAmountChangedIntegrationEvent : IntegrationEvent
    {
        public int ScholarshipItemId { get; private init; }

        public decimal NewAmount { get; private init; }

        public decimal OldAmount { get; private init; }

        public ScholarshipItemAmountChangedIntegrationEvent(int scholarshipItemId, decimal newAmount, decimal oldAmount)
        {
            ScholarshipItemId = scholarshipItemId;
            NewAmount = newAmount;
            OldAmount = oldAmount;
        }
    }
}
