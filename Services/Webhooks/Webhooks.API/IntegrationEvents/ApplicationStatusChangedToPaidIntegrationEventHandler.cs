using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.IntegrationEvents
{
    public class ApplicationStatusChangedToPaidIntegrationEventHandler : IIntegrationEventHandler<ApplicationStatusChangedToPaidIntegrationEvent>
    {
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;
        private readonly ILogger _logger;
        public ApplicationStatusChangedToPaidIntegrationEventHandler(IWebhooksRetriever retriever, IWebhooksSender sender, ILogger<ApplicationStatusChangedToGrantedIntegrationEventHandler> logger)
        {
            _retriever = retriever;
            _sender = sender;
            _logger = logger;
        }

        public async Task Handle(ApplicationStatusChangedToPaidIntegrationEvent @event)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.ApplicationPaid);
            _logger.LogInformation("Received ApplicationStatusChangedToPaidIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.ApplicationPaid, @event);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
