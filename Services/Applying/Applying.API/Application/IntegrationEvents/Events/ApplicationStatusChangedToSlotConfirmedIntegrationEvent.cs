namespace Applying.API.Application.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record ApplicationStatusChangedToSlotConfirmedIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public string ApplicationStatus { get; }
        public string StudentName { get; }

        public ApplicationStatusChangedToSlotConfirmedIntegrationEvent(int applicationId, string applicationStatus, string studentName)
        {
            ApplicationId = applicationId;
            ApplicationStatus = applicationStatus;
            StudentName = studentName;
        }
    }
}