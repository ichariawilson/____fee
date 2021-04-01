using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using System.Threading.Tasks;

namespace Scholarship.API.IntegrationEvents
{
    public interface IScholarshipIntegrationEventService
    {
        Task SaveEventAndScholarshipContextChangesAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
