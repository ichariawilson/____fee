namespace Payment.API.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record ApplicationPaymentFailedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public ApplicationPaymentFailedIntegrationEvent(int applicationId) => ApplicationId = applicationId;
    }
}