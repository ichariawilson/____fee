using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.API.Application.IntegrationEvents.Events
{
    public record ApplicationStatusChangedToSubmittedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }

        public ApplicationStatusChangedToSubmittedIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
