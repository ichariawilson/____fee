using Applying.Basket.API.IntegrationEvents.Events;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.Services.Applying.Basket.API.Model;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Threading.Tasks;

namespace Applying.Basket.API.IntegrationEvents.EventHandling
{
    public class ApplicationStartedIntegrationEventHandler : IIntegrationEventHandler<ApplicationStartedIntegrationEvent>
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<ApplicationStartedIntegrationEventHandler> _logger;

        public ApplicationStartedIntegrationEventHandler(
            IBasketRepository repository,
            ILogger<ApplicationStartedIntegrationEventHandler> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationStartedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                await _repository.DeleteBasketAsync(@event.UserId.ToString());
            }
        }
    }
}



