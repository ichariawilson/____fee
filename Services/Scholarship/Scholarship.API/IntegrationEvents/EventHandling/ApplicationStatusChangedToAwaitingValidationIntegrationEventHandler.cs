namespace Microsoft.Fee.Services.Scholarship.API.IntegrationEvents.EventHandling
{
    using BuildingBlocks.EventBus.Abstractions;
    using BuildingBlocks.EventBus.Events;
    using global::Scholarship.API.IntegrationEvents;
    using Infrastructure;
    using IntegrationEvents.Events;
    using Microsoft.Extensions.Logging;
    using Serilog.Context;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly ScholarshipContext _scholarshipContext;
        private readonly IScholarshipIntegrationEventService _scholarshipIntegrationEventService;
        private readonly ILogger<ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler(
            ScholarshipContext scholarshipContext,
            IScholarshipIntegrationEventService scholarshipIntegrationEventService,
            ILogger<ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _scholarshipContext = scholarshipContext;
            _scholarshipIntegrationEventService = scholarshipIntegrationEventService;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationStatusChangedToAwaitingValidationIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var confirmedApplicationSlotItems = new List<ConfirmedApplicationSlotItem>();

                foreach (var applicationSlotItem in @event.ApplicationSlotItems)
                {
                    var scholarshipItem = _scholarshipContext.ScholarshipItems.Find(applicationSlotItem.ScholarshipItemId);
                    var hasSlots = scholarshipItem.AvailableSlots >= applicationSlotItem.Slots;
                    var confirmedApplicationSlotItem = new ConfirmedApplicationSlotItem(scholarshipItem.Id, hasSlots);

                    confirmedApplicationSlotItems.Add(confirmedApplicationSlotItem);
                }

                var confirmedIntegrationEvent = confirmedApplicationSlotItems.Any(c => !c.HasSlots)
                    ? (IntegrationEvent)new ApplicationSlotRejectedIntegrationEvent(@event.ApplicationId, confirmedApplicationSlotItems)
                    : new ApplicationSlotConfirmedIntegrationEvent(@event.ApplicationId);

                await _scholarshipIntegrationEventService.SaveEventAndScholarshipContextChangesAsync(confirmedIntegrationEvent);
                await _scholarshipIntegrationEventService.PublishThroughEventBusAsync(confirmedIntegrationEvent);

            }
        }
    }
}