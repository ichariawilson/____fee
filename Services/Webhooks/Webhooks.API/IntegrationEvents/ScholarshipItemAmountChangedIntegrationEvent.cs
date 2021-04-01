using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Webhooks.API.IntegrationEvents
{
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
