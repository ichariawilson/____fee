using MediatR;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.BuildingBlocks.EventBus.Extensions;
using Microsoft.Fee.Services.Applying.API.Application.Commands;
using Microsoft.Extensions.Logging;
using Applying.API.Application.IntegrationEvents.Events;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Applying.API.Application.IntegrationEvents.EventHandling
{
    public class UserCheckoutAcceptedIntegrationEventHandler : IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserCheckoutAcceptedIntegrationEventHandler> _logger;

        public UserCheckoutAcceptedIntegrationEventHandler(
            IMediator mediator,
            ILogger<UserCheckoutAcceptedIntegrationEventHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Integration event handler which starts the create Application process
        /// </summary>
        /// <param name="@event">
        /// Integration event message which is sent by the
        /// basket.api once it has successfully process the 
        /// Application items.
        /// </param>
        /// <returns></returns>
        public async Task Handle(UserCheckoutAcceptedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var result = false;

                if (@event.RequestId != Guid.Empty)
                {
                    using (LogContext.PushProperty("IdentifiedCommandId", @event.RequestId))
                    {
                        var createApplicationCommand = new CreateApplicationCommand(@event.Basket.Items, @event.UserId, @event.UserName, @event.IDNumber, @event.Request, @event.PaymentTypeId);

                        var requestCreateApplication = new IdentifiedCommand<CreateApplicationCommand, bool>(createApplicationCommand, @event.RequestId);

                        _logger.LogInformation(
                            "----- Sending command: {CommandName} - {IdProperty}: {CommandId} ({@Command})",
                            requestCreateApplication.GetGenericTypeName(),
                            nameof(requestCreateApplication.Id),
                            requestCreateApplication.Id,
                            requestCreateApplication);

                        result = await _mediator.Send(requestCreateApplication);

                        if (result)
                        {
                            _logger.LogInformation("----- CreateApplicationCommand suceeded - RequestId: {RequestId}", @event.RequestId);
                        }
                        else
                        {
                            _logger.LogWarning("CreateApplicationCommand failed - RequestId: {RequestId}", @event.RequestId);
                        }
                    }
                }
                else
                {
                    _logger.LogWarning("Invalid IntegrationEvent - RequestId is missing - {@IntegrationEvent}", @event);
                }
            }
        }
    }
}