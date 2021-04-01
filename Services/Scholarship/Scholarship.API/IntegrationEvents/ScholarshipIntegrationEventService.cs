using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Fee.BuildingBlocks.EventBus.Events;
using Microsoft.Fee.BuildingBlocks.IntegrationEventLogEF.Services;
using Microsoft.Fee.BuildingBlocks.IntegrationEventLogEF.Utilities;
using Microsoft.Fee.Services.Scholarship.API.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Scholarship.API.IntegrationEvents
{
    public class ScholarshipIntegrationEventService : IScholarshipIntegrationEventService, IDisposable
    {
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly IEventBus _eventBus;
        private readonly ScholarshipContext _scholarshipContext;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly ILogger<ScholarshipIntegrationEventService> _logger;
        private volatile bool disposedValue;

        public ScholarshipIntegrationEventService(
            ILogger<ScholarshipIntegrationEventService> logger,
            IEventBus eventBus,
            ScholarshipContext scholarshipContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _scholarshipContext = scholarshipContext ?? throw new ArgumentNullException(nameof(scholarshipContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = _integrationEventLogServiceFactory(_scholarshipContext.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            try
            {
                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId_published} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsInProgressAsync(evt.Id);
                _eventBus.Publish(evt);
                await _eventLogService.MarkEventAsPublishedAsync(evt.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);
                await _eventLogService.MarkEventAsFailedAsync(evt.Id);
            }
        }

        public async Task SaveEventAndScholarshipContextChangesAsync(IntegrationEvent evt)
        {
            _logger.LogInformation("----- ScholarshipIntegrationEventService - Saving changes and integrationEvent: {IntegrationEventId}", evt.Id);

            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency            
            await ResilientTransaction.New(_scholarshipContext).ExecuteAsync(async () =>
            {
                // Achieving atomicity between original scholarship database operation and the IntegrationEventLog thanks to a local transaction
                await _scholarshipContext.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _scholarshipContext.Database.CurrentTransaction);
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    (_eventLogService as IDisposable)?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
