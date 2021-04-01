using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.SignalrHub.IntegrationEvents.Events
{
    public record ApplicationStatusChangedToGrantedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }

        public ApplicationStatusChangedToGrantedIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
