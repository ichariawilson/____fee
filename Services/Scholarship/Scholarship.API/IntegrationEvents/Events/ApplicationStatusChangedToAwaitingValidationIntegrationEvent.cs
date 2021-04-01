namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events
{
    using BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public record ApplicationStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public IEnumerable<ApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationStatusChangedToAwaitingValidationIntegrationEvent(int applicationId,
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