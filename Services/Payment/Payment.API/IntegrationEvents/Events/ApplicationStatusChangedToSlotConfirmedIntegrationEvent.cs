namespace Payment.API.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record ApplicationStatusChangedToSlotConfirmedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public ApplicationStatusChangedToSlotConfirmedIntegrationEvent(int applicationId)
            => ApplicationId = applicationId;
    }
}