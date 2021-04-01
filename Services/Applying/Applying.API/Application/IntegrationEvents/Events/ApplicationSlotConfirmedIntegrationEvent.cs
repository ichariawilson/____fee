namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record ApplicationSlotConfirmedIntegrationEvent : IntegrationEvent
    {
        public int Applicationd { get; }

        public ApplicationSlotConfirmedIntegrationEvent(int applicationId) => Applicationd = applicationId;
    }
}