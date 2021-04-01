namespace Applying.API.Application.DomainEventHandlers.ApplicationPaid
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

    public class ApplicationStatusChangedToPaidDomainEventHandler
                   : INotificationHandler<ApplicationStatusChangedToPaidDomainEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILoggerFactory _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;


        public ApplicationStatusChangedToPaidDomainEventHandler(
            IApplicationRepository applicationRepository, ILoggerFactory logger,
            IStudentRepository studentRepository,
            IApplyingIntegrationEventService applyingIntegrationEventService
            )
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _applyingIntegrationEventService = applyingIntegrationEventService ?? throw new ArgumentNullException(nameof(applyingIntegrationEventService));
        }

        public async Task Handle(ApplicationStatusChangedToPaidDomainEvent applicationStatusChangedToPaidDomainEvent, CancellationToken cancellationToken)
        {
            _logger.CreateLogger<ApplicationStatusChangedToPaidDomainEventHandler>()
                .LogTrace("Application with Id: {ApplicationId} has been successfully updated to status {Status} ({Id})",
                    applicationStatusChangedToPaidDomainEvent.ApplicationId, nameof(ApplicationStatus.Paid), ApplicationStatus.Paid.Id);

            var application = await _applicationRepository.GetAsync(applicationStatusChangedToPaidDomainEvent.ApplicationId);
            var student = await _studentRepository.FindByIdAsync(application.GetStudentId.Value.ToString());

            var applicationSlotList = applicationStatusChangedToPaidDomainEvent.ApplicationItems
                .Select(applicationItem => new ApplicationSlotItem(applicationItem.ScholarshipItemId, applicationItem.GetSlots()));

            var applicationStatusChangedToPaidIntegrationEvent = new ApplicationStatusChangedToPaidIntegrationEvent(
                applicationStatusChangedToPaidDomainEvent.ApplicationId,
                application.ApplicationStatus.Name,
                student.UserName,
                applicationSlotList);

            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStatusChangedToPaidIntegrationEvent);
        }
    }
}