using Microsoft.AspNetCore.SignalR;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Applying.SignalrHub.IntegrationEvents
{
    public class ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler : IIntegrationEventHandler<ApplicationStatusChangedToAwaitingValidationIntegrationEvent>
    {
        private readonly IHubContext<NotificationsHub> _hubContext;
        private readonly ILogger<ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler> _logger;

        public ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler(
            IHubContext<NotificationsHub> hubContext,
            ILogger<ApplicationStatusChangedToAwaitingValidationIntegrationEventHandler> logger)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task Handle(ApplicationStatusChangedToAwaitingValidationIntegrationEvent @event)
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
