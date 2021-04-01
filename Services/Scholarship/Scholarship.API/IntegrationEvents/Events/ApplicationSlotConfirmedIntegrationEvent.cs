namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events
{
    using BuildingBlocks.EventBus.Events;

    public record ApplicationSlotConfirmedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public ApplicationSlotConfirmedIntegrationEvent(int applicationId) => ApplicationId = applicationId;
    }
}