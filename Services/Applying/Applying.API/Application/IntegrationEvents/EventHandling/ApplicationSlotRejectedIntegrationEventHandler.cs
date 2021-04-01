namespace Applying.API.Application.IntegrationEvents.EventHandling
{
    using Events;
    using MediatR;
    using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
    using Microsoft.Extensions.Logging;
    using Applying.API.Application.Commands;
    using Serilog.Context;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApplicationSlotRejectedIntegrationEventHandler : IIntegrationEventHandler<ApplicationSlotRejectedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplicationSlotRejectedIntegrationEventHandler> _logger;

        public ApplicationSlotRejectedIntegrationEventHandler(
            IMediator mediator,
            ILogger<ApplicationSlotRejectedIntegrationEventHandler> logger)
        {
            _mediator = mediator;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationSlotRejectedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var applicationSlotRejectedItems = @event.ApplicationSlotItems
                    .FindAll(c => !c.HasSlots)
                    .Select(c => c.ScholarshipItemId)
                    .ToList();

                var command = new SetSlotRejectedApplicationStatusCommand(@event.ApplicationId, applicationSlotRejectedItems);

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