namespace Applying.BackgroundTasks.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;

    public record GracePeriodConfirmedIntegrationEvent : IntegrationEvent
    {
        public int ScholarshipId { get; }

        public GracePeriodConfirmedIntegrationEvent(int scholarshipId) =>
            ScholarshipId = scholarshipId;
    }
}
