namespace Applying.API.Application.IntegrationEvents.EventHandling
{
    using Events;
    using MediatR;
    using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
    using Microsoft.Extensions.Logging;
    using Applying.API.Application.Commands;
    using Serilog.Context;
    using System;
    using System.Threading.Tasks;

    public class ApplicationSlotConfirmedIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationSlotConfirmedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationSlotConfirmedIntegrationEventHandler> _logger;

        public ApplicationSlotConfirmedIntegrationEventHandler(
            IMediator mediator,
            ILogger<ApplicationSlotConfirmedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationSlotConfirmedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var command = new SetSlotConfirmedApplicationStatusCommand(@event.Applicationd);

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