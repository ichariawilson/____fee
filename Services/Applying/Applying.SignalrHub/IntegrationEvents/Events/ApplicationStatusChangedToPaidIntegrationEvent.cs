using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.SignalrHub.IntegrationEvents.Events
{
    public record ApplicationStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }

        public ApplicationStatusChangedToPaidIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
