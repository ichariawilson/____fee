using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Applying.SignalrHub.IntegrationEvents
{
    public record ApplicationStatusChangedToAwaitingValidationIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }

        public ApplicationStatusChangedToAwaitingValidationIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
