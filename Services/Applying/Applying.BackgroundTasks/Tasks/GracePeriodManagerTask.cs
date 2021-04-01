﻿using Dapper;
using Microsoft.Fee.BuildingBlocks.EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Applying.BackgroundTasks.Events;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;

namespace Applying.BackgroundTasks.Tasks
{
    public class GracePeriodManagerService : BackgroundService
    {
        private readonly ILogger<GracePeriodManagerService> _logger;
        private readonly BackgroundTaskSettings _settings;
        private readonly IEventBus _eventBus;

        public GracePeriodManagerService(IOptions<BackgroundTaskSettings> settings, IEventBus eventBus, ILogger<GracePeriodManagerService> logger)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("GracePeriodManagerService is starting.");

            stoppingToken.Register(() => _logger.LogDebug("#1 GracePeriodManagerService background task is stopping."));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("GracePeriodManagerService background task is doing background work.");

                CheckConfirmedGracePeriodApplications();

                await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }

            _logger.LogDebug("GracePeriodManagerService background task is stopping.");
        }

        private void CheckConfirmedGracePeriodApplications()
        {
            _logger.LogDebug("Checking confirmed grace period applications");

            var applicationIds = GetConfirmedGracePeriodApplications();

            foreach (var applicationId in applicationIds)
            {
                var confirmGracePeriodEvent = new GracePeriodConfirmedIntegrationEvent(applicationId);

                _logger.LogInformation("----- Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", confirmGracePeriodEvent.Id, Program.AppName, confirmGracePeriodEvent);

                _eventBus.Publish(confirmGracePeriodEvent);
            }
        }

        private IEnumerable<int> GetConfirmedGracePeriodApplications()
        {
            IEnumerable<int> applicationIds = new List<int>();

            using (var conn = new SqlConnection(_settings.ConnectionString))
            {
                try
                {
                    conn.Open();
                    applicationIds = conn.Query<int>(
                        @"SELECT Id FROM [applying].[applications] 
                            WHERE DATEDIFF(minute, [ApplicationDate], GETDATE()) >= @GracePeriodTime
                            AND [ApplicationStatusId] = 1",
                        new { _settings.GracePeriodTime });
                }
                catch (SqlException exception)
                {
                    _logger.LogCritical(exception, "FATAL ERROR: Database connections could not be opened: {Message}", exception.Message);
                }

            }

            return applicationIds;
        }
    }
}
