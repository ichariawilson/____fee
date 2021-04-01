using Microsoft.AspNetCore.SignalR;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Applying.SignalrHub.IntegrationEvents.Events;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Applying.SignalrHub.IntegrationEvents.EventHandling
{
    public class ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationStatusChangedToSlotConfirmedIntegrationEvent>
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly ILogger<ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler> _logger;

        public ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler(
            IHubContext<NotificationsHub> hubContext,
            ILogger<ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task Handle(ApplicationStatusChangedToSlotConfirmedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                await _hubContext.Clients
                    .Group(@event.StudentName)
                    .SendAsync("UpdatedApplicationState", new { ApplicationId = @event.ApplicationId, Status = @event.ApplicationStatus });
            }
        }
    }
}
