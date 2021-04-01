namespace Applying.API.Application.DomainEventHandlers.ApplicationGracePeriodConfirmed
{
    using Domain.Events;
    using MediatR;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
    using Microsoft.Extensions.Logging;
    using Applying.API.Application.IntegrationEvents;
    using Applying.API.Application.IntegrationEvents.Events;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApplicationStatusChangedToAwaitingValidationDomainEventHandler
                   : INotificationHandler<ApplicationStatusChangedToAwaitingValidationDomainEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILoggerFactory _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;

        public ApplicationStatusChangedToAwaitingValidationDomainEventHandler(
            IApplicationRepository applicationRepository, ILoggerFactory logger,
            IStudentRepository studentRepository,
            IApplyingIntegrationEventService applyingIntegrationEventService)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _studentRepository = studentRepository;
            _applyingIntegrationEventService = applyingIntegrationEventService;
        }

        public async Task Handle(ApplicationStatusChangedToAwaitingValidationDomainEvent applicationStatusChangedToAwaitingValidationDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ApplicationStatusChangedToAwaitingValidationDomainEvent>()
                .LogTrace("Application with Id: {ApplicationId} has been successfully updated to status {Status} ({Id})",
                    applicationStatusChangedToAwaitingValidationDomainEvent.ApplicationId, nameof(ApplicationStatus.AwaitingValidation), ApplicationStatus.AwaitingValidation.Id);

            var application = await _applicationRepository.GetAsync(applicationStatusChangedToAwaitingValidationDomainEvent.ApplicationId);

            var student = await _studentRepository.FindByIdAsync(application.GetStudentId.Value.ToString());

            var applicationSlotList = applicationStatusChangedToAwaitingValidationDomainEvent.ApplicationItems
                .Select(applicationItem => new ApplicationSlotItem(applicationItem.ScholarshipItemId, applicationItem.GetSlots()));

            var applicationStatusChangedToAwaitingValidationIntegrationEvent = new ApplicationStatusChangedToAwaitingValidationIntegrationEvent(
                application.Id, application.ApplicationStatus.Name, student.UserName, applicationSlotList);
            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStatusChangedToAwaitingValidationIntegrationEvent);
        }
    }
}