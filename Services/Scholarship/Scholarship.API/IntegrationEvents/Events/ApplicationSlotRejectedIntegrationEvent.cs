namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events
{
    using BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public record ApplicationSlotRejectedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }

        public List<ConfirmedApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationSlotRejectedIntegrationEvent(int applicationId,
            List<ConfirmedApplicationSlotItem> applicationSlotItems)
        {
            ApplicationId = applicationId;
            ApplicationSlotItems = applicationSlotItems;
        }
    }

    public record ConfirmedApplicationSlotItem
    {
        public int ScholarshipItemId { get; }
        public bool HasSlots { get; }

        public ConfirmedApplicationSlotItem(int scholarshipItemId, bool hasSlots)
        {
            ScholarshipItemId = scholarshipItemId;
            HasSlots = hasSlots;
        }
    }
}