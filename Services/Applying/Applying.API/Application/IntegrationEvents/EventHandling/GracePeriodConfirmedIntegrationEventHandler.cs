using MediatR;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
using Microsoft.Extensions.Logging;
using Applying.API.Application.Commands;
using Applying.API.Application.IntegrationEvents.Events;
using Serilog.Context;
using System.Threading.Tasks;

namespace Applying.API.Application.IntegrationEvents.EventHandling
{
    public class GracePeriodConfirmedIntegrationEventHandler : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GracePeriodConfirmedIntegrationEventHandler> _logger;

        public GracePeriodConfirmedIntegrationEventHandler(
            IMediator mediator,
            ILogger<GracePeriodConfirmedIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Event handler which confirms that the grace period
        /// has been completed and Application will not initially be cancelled.
        /// Therefore, the Application process continues for validation. 
        /// </summary>
        /// <param name="event">       
        /// </param>
        /// <returns></returns>
        public async Task Handle(GracePeriodConfirmedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var command = new SetAwaitingValidationApplicationStatusCommand(@event.ApplicationId);

                _logger.LogInformation(
                    "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                    command.GetGenericTypeName(),
                    nameof(command.ApplicationNumber),
                    command.ApplicationNumber,
                    command);

                await _mediator.Send(command);
            }
        }
    }
}
