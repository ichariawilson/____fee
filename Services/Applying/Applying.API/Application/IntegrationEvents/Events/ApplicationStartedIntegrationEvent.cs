using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.API.Application.IntegrationEvents.Events
{
    // Integration Events notes: 
    // An Event is “something that has happened in the past”, therefore its name has to be   
    // An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    public record ApplicationStartedIntegrationEvent : IntegrationEvent
    {
        public string UserId { get; set; }

        public ApplicationStartedIntegrationEvent(string userId)
            => UserId = userId;
    }
}