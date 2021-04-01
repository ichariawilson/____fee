namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using Infrastructure;
    using Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.Events;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;
    using System.Threading.Tasks;

    public class ApplicationStatusChangedToPaidIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationStatusChangedToPaidIntegrationEvent>
    {
        private readonly ScholarshipContext _scholarshipContext;
        private readonly ILogger<ApplicationStatusChangedToPaidIntegrationEventHandler> _logger;

        public ApplicationStatusChangedToPaidIntegrationEventHandler(
            ScholarshipContext scholarshipContext,
            ILogger<ApplicationStatusChangedToPaidIntegrationEventHandler> logger)
        {
            _scholarshipContext = scholarshipContext;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationStatusChangedToPaidIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                //we're not blocking slots
                foreach (var applicationSlotItem in @event.ApplicationSlotItems)
                {
                    var scholarshipItem = _scholarshipContext.ScholarshipItems.Find(applicationSlotItem.ScholarshipItemId);

                    scholarshipItem.RemoveSlots(applicationSlotItem.Slots);
                }

                await _scholarshipContext.SaveChangesAsync();

            }
        }
    }
}