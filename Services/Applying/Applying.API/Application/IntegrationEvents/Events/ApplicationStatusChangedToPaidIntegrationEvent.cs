namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public record ApplicationStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }
        public IEnumerable<ApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationStatusChangedToPaidIntegrationEvent(int applicationId,
            string applicationStatus,
            string studentName,
            IEnumerable<ApplicationSlotItem> applicationSlotItems)
        {
            ApplicationId = applicationId;
            ApplicationSlotItems = applicationSlotItems;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
