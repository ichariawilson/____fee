namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public record ApplicationStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }
        public IEnumerable<ApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationStatusChangedToAwaitingValidationIntegrationEvent(int applicationId, string applicationStatus, string studentName,
            IEnumerable<ApplicationSlotItem> applicationSlotItems)
        {
            ApplicationId = applicationId;
            ApplicationSlotItems = applicationSlotItems;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
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