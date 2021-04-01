using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.Services.Applying.Basket.API.IntegrationEvents.Events;
using Microsoft.Fee.Services.Applying.Basket.API.Model;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Fee.Services.Applying.Basket.API.IntegrationEvents.EventHandling
{
    public class ScholarshipItemAmountChangedIntegrationEventHandler : IIntegrationEventHandler<ScholarshipItemAmountChangedIntegrationEvent>
    {
        private readonly ILogger<ScholarshipItemAmountChangedIntegrationEventHandler> _logger;
        private readonly IBasketRepository _repository;

        public ScholarshipItemAmountChangedIntegrationEventHandler(
            ILogger<ScholarshipItemAmountChangedIntegrationEventHandler> logger,
            IBasketRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task Handle(ScholarshipItemAmountChangedIntegrationEvent @event)
        {
            using (LogContext.PushProperty("IntegrationEventContext", $"{@event.Id}-{Program.AppName}"))
            {
                _logger.LogInformation("----- Handling integration event: {IntegrationEventId} at {AppName} - ({@IntegrationEvent})", @event.Id, Program.AppName, @event);

                var userIds = _repository.GetUsers();

                foreach (var id in userIds)
                {
                    var basket = await _repository.GetBasketAsync(id);

                    await UpdateAmountInBasketItems(@event.ScholarshipItemId, @event.NewAmount, @event.OldAmount, basket);
                }
            }
        }

        private async Task UpdateAmountInBasketItems(int scholarshipItemId, decimal newAmount, decimal oldAmount, StudentBasket basket)
        {
            var itemsToUpdate = basket?.Items?.Where(x => x.ScholarshipItemId == scholarshipItemId).ToList();

            if (itemsToUpdate != null)
            {
                _logger.LogInformation("----- ScholarshipItemAmountChangedIntegrationEventHandler - Updating items in basket for user: {Studentd} ({@Items})", basket.StudentId, itemsToUpdate);

                foreach (var item in itemsToUpdate)
                {
                    if (item.SlotAmount == oldAmount)
                    {
                        var originalAmount = item.SlotAmount;
                        item.SlotAmount = newAmount;
                        item.OldSlotAmount = originalAmount;
                    }
                }
                await _repository.UpdateBasketAsync(basket);
            }
        }
    }
}

