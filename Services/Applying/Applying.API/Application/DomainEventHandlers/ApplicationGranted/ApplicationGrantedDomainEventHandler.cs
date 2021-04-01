using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Extensions.Logging;
using Applying.API.Application.IntegrationEvents;
using Applying.API.Application.IntegrationEvents.Events;
using Applying.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Applying.API.Application.DomainEventHandlers.ApplicationGranted
{
    public class ApplicationGrantedDomainEventHandler
                   : INotificationHandler<ApplicationGrantedDomainEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;
        private readonly ILoggerFactory _logger;

        public ApplicationGrantedDomainEventHandler(
            IApplicationRepository applicationRepository,
            ILoggerFactory logger,
            IStudentRepository studentRepository,
            IApplyingIntegrationEventService applyingIntegrationEventService)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _applyingIntegrationEventService = applyingIntegrationEventService;
        }

        public async Task Handle(ApplicationGrantedDomainEvent applicationGrantedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ApplicationGrantedDomainEvent>()
                .LogTrace("Application with Id: {ApplicationId} has been successfully updated to status {Status} ({Id})",
                    applicationGrantedDomainEvent.Application.Id, nameof(ApplicationStatus.Granted), ApplicationStatus.Granted.Id);

            var application = await _applicationRepository.GetAsync(applicationGrantedDomainEvent.Application.Id);
            var student = await _studentRepository.FindByIdAsync(application.GetStudentId.Value.ToString());

            var applicationStatusChangedToGrantedIntegrationEvent = new ApplicationStatusChangedToGrantedIntegrationEvent(application.Id, application.ApplicationStatus.Name, student.UserName);
            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStatusChangedToGrantedIntegrationEvent);
        }
    }
}
