using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Webhooks.API.Model;
using Webhooks.API.Services;

namespace Webhooks.API.IntegrationEvents
{
    public class ScholarshipItemAmountChangedIntegrationEventHandler : IIntegrationEventHandler<ScholarshipItemAmountChangedIntegrationEvent>
    {
        private readonly IWebhooksRetriever _retriever;
        private readonly IWebhooksSender _sender;
        private readonly ILogger _logger;
        public ScholarshipItemAmountChangedIntegrationEventHandler(IWebhooksRetriever retriever, IWebhooksSender sender, ILogger<ApplicationStatusChangedToGrantedIntegrationEventHandler> logger)
        {
            _retriever = retriever;
            _sender = sender;
            _logger = logger;
        }
        public async Task Handle(ScholarshipItemAmountChangedIntegrationEvent @event)
        {
            var subscriptions = await _retriever.GetSubscriptionsOfType(WebhookType.ScholarshipItemAmountChange);
            _logger.LogInformation("Received ScholarshipItemAmountChangedIntegrationEvent and got {SubscriptionsCount} subscriptions to process", subscriptions.Count());
            var whook = new WebhookData(WebhookType.ScholarshipItemAmountChange, @event);
            await _sender.SendAll(subscriptions, whook);
        }
    }
}
