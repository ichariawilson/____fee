using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.Basket.API.IntegrationEvents.Events
{
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record ApplicationStartedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; init; }

        public ApplicationStartedIntegrationEvent(string userId)
            => UserId = userId;
    }
}
