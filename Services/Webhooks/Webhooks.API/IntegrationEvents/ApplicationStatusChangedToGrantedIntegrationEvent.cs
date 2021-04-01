using Microsoft.Fee.BuildingBlocks.EventBus.Events;

namespace Webhooks.API.IntegrationEvents
{
    public record ApplicationStatusChangedToGrantedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; private init; }
        public string ApplicationStatus { get; private init; }
        public string StudentName { get; private init; }

        public ApplicationStatusChangedToGrantedIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}
