using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using System.Collections.Generic;

namespace Webhooks.API.IntegrationEvents
{
    public record ApplicationStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public IEnumerable<ApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationStatusChangedToPaidIntegrationEvent(int applicationId,
            IEnumerable<ApplicationSlotItem> applicationSlotItems)
        {
            ApplicationId = applicationId;
            ApplicationSlotItems = applicationSlotItems;
        }
    }

    public record ApplicationSlotItem
    {
        public int ScholarshipItemId { get; }
        public int Slots { get; }

        public ApplicationSlotItem(int scholarshipItemId, int slots)
        {
            ScholarshipItemId = scholarshipItemId;
            Slots = slots;
        }
    }
}
