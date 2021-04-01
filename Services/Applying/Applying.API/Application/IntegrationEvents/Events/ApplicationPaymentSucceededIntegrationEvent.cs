namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record ApplicationPaymentSucceededIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public ApplicationPaymentSucceededIntegrationEvent(int applicationId) => ApplicationId = applicationId;
    }
}