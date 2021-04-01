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

    public class ApplicationPaymentSucceededIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationPaymentSucceededIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationPaymentSucceededIntegrationEventHandler> _logger;

        public ApplicationPaymentSucceededIntegrationEventHandler(
            IMediator mediator,
            ILogger<ApplicationPaymentSucceededIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationPaymentSucceededIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var command = new SetPaidApplicationStatusCommand(@event.ApplicationId);

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