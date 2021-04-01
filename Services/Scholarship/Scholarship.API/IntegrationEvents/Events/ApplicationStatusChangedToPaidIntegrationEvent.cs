namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;
    using System.Collections.Generic;

    public record ApplicationStatusChangedToPaidIntegrationEvent : IntegrationEvent
    {
        public int ApplicationId { get; }
        public IEnumerable<ApplicationSlotItem> ApplicationSlotItems { get; }

        public ApplicationStatusChangedToPaidIntegrationEvent(int applicationId,
            IEnumerable<ApplicationSlotItem> applicationSlotItems)
        {
            ApplicationId = applicationId;
            ApplicationSlotItems = applicationSlotItems;
        }
    }
}