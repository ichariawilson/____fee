using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using System;
using System.Threading.Tasks;

namespace Applying.API.Application.IntegrationEvents
{
    public interface IApplyingIntegrationEventService
    {
        Task PublishEventsThroughEventBusAsync(Guid transactionId);
        Task AddAndSaveEventAsync(IntegrationEvent evt);
    }
}
