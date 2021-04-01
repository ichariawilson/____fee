namespace Payment.API.IntegrationEvents.EventHandling
{
    using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
    using Microsoft.Fee.BuildingBlocks.EventBus.Events;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Payment.API.IntegrationEvents.Events;
    using Serilog.Context;
    using System.Threading.Tasks;

    public class ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler :
        IIntegrationEventHandler<ApplicationStatusChangedToSlotConfirmedIntegrationEvent>
    {
        private readonly IEventBus _eventBus;
        private readonly PaymentSettings _settings;
        private readonly ILogger<ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler> _logger;

        public ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler(
            IEventBus eventBus,
            IOptionsSnapshot<PaymentSettings> settings,
            ILogger<ApplicationStatusChangedToSlotConfirmedIntegrationEventHandler> logger)
        {
            _eventBus = eventBus;
            _settings = settings.Value;
            _logger = logger ?? throw new System.ArgumentNullException(nameof(logger));

            _logger.LogTrace("PaymentSettings: {@PaymentSettings}", _settings);
        }

        public async Task Handle(ApplicationStatusChangedToSlotConfirmedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                IntegrationEvent applicationPaymentIntegrationEvent;

                //Business feature comment:
                // When ApplicationStatusChangedToSlotConfirmed Integration Event is handled.
                // Here we're simulating that we'd be performing the payment against any payment gateway
                // Instead of a real payment we just take the env. var to simulate the payment 
                // The payment can be successful or it can fail

                if (_settings.PaymentSucceeded)
                {
                    applicationPaymentIntegrationEvent = new ApplicationPaymentSucceededIntegrationEvent(@event.ApplicationId);
                }
                else
                {
                    applicationPaymentIntegrationEvent = new ApplicationPaymentFailedIntegrationEvent(@event.ApplicationId);
                }

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", applicationPaymentIntegrationEvent.Id, Program.AppName, applicationPaymentIntegrationEvent);

                _eventBus.Publish(applicationPaymentIntegrationEvent);

                await Task.CompletedTask;
            }
        }
    }
}