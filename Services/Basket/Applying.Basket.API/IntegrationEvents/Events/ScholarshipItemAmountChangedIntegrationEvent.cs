using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Microsoft.Fee.Services.Applying.Basket.API.IntegrationEvents.Events
{ 
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
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
