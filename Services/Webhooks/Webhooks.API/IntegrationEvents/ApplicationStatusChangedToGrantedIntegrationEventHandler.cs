using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.IntegrationEvents
{
    public class ApplicationStatusChangedToGrantedIntegrationEventHandler : IIntegrationEventHandler<ApplicationStatusChangedToGrantedIntegrationEvent>
    {
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;
        private readonly ILogger _logger;
        public ApplicationStatusChangedToGrantedIntegrationEventHandler(IWebhooksRetriever retriever, IWebhooksSender sender, ILogger<ApplicationStatusChangedToGrantedIntegrationEventHandler> logger)
        {
            _retriever = retriever;
            _sender = sender;
            _logger = logger;
        }

        public async Task Handle(ApplicationStatusChangedToGrantedIntegrationEvent @event)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.ApplicationGranted);
            _logger.LogInformation("Received ApplicationStatusChangedToGrantedIntegrationEvent and got {SubscriptionCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.ApplicationGranted, @event);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
