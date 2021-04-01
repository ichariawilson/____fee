namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public GracePeriodConfirmedIntegrationEvent(int applicationId) =>
            ApplicationId = applicationId;
    }
}
