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

    public class ApplicationPaymentFailedIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationPaymentFailedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationPaymentFailedIntegrationEventHandler> _logger;

        public ApplicationPaymentFailedIntegrationEventHandler(
            IMediator mediator,
            ILogger<ApplicationPaymentFailedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationPaymentFailedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var command = new CancelApplicationCommand(@event.ApplicationId);

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
