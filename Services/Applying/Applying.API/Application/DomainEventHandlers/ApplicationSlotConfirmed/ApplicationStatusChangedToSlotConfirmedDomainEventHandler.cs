namespace Applying.API.Application.DomainEventHandlers.ApplicationSlotConfirmed
{
    using Domain.Events;
    using MediatR;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
    using Microsoft.Extensions.Logging;
    using Applying.API.Application.IntegrationEvents;
    using Applying.API.Application.IntegrationEvents.Events;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApplicationStatusChangedToSlotConfirmedDomainEventHandler
                   : INotificationHandler<ApplicationStatusChangedToSlotConfirmedDomainEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILoggerFactory _logger;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;

        public ApplicationStatusChangedToSlotConfirmedDomainEventHandler(
            IApplicationRepository applicationRepository,
            IStudentRepository studentRepository,
            ILoggerFactory logger,
            IApplyingIntegrationEventService applyingIntegrationEventService)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applyingIntegrationEventService = applyingIntegrationEventService;
        }

        public async Task Handle(ApplicationStatusChangedToSlotConfirmedDomainEvent applicationStatusChangedToSlotConfirmedDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ApplicationStatusChangedToSlotConfirmedDomainEventHandler>()
                .LogTrace("Application with Id: {ApplicationId} has been successfully updated to status {Status} ({Id})",
                    applicationStatusChangedToSlotConfirmedDomainEvent.ApplicationId, nameof(ApplicationStatus.SlotConfirmed), ApplicationStatus.SlotConfirmed.Id);

            var application = await _applicationRepository.GetAsync(applicationStatusChangedToSlotConfirmedDomainEvent.ApplicationId);
            var student = await _studentRepository.FindByIdAsync(application.GetStudentId.Value.ToString());

            var applicationStatusChangedToSlotConfirmedIntegrationEvent = new ApplicationStatusChangedToSlotConfirmedIntegrationEvent(application.Id, application.ApplicationStatus.Name, student.UserName);
            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStatusChangedToSlotConfirmedIntegrationEvent);
        }
    }
}