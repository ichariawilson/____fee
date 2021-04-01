using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Extensions.Logging;
using Applying.API.Application.IntegrationEvents;
using Applying.API.Application.IntegrationEvents.Events;
using Applying.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Fee.Services.Applying.API.Infrastructure.Services;

namespace Applying.API.Application.DomainEventHandlers.ApplicationStartedEvent
{
    public class ValidateOrAddStudentAggregateWhenApplicationStartedDomainEventHandler
                        : INotificationHandler<ApplicationStartedDomainEvent>
    {
        private readonly ILoggerFactory _logger;
        private readonly IStudentRepository _studentRepository;
        private readonly IIdentityService _identityService;
        private readonly IApplyingIntegrationEventService _applyingIntegrationEventService;

        public ValidateOrAddStudentAggregateWhenApplicationStartedDomainEventHandler(
            ILoggerFactory logger,
            IStudentRepository studentRepository,
            IIdentityService identityService,
            IApplyingIntegrationEventService applyingIntegrationEventService)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _identityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
            _applyingIntegrationEventService = applyingIntegrationEventService ?? throw new ArgumentNullException(nameof(applyingIntegrationEventService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Handle(ApplicationStartedDomainEvent applicationStartedEvent, CancellationToken cancellationToken)
        {
            var paymentTypeId = (applicationStartedEvent.PaymentTypeId != 0) ? applicationStartedEvent.PaymentTypeId : 1;
            var student = await _studentRepository.FindAsync(applicationStartedEvent.UserId);
            bool studentOriginallyExisted = (student == null) ? false : true;

            if (!studentOriginallyExisted)
            {
                student = new Student(applicationStartedEvent.UserId, applicationStartedEvent.UserName);
            }

            student.VerifyOrAddPaymentMethod(paymentTypeId, applicationStartedEvent.Application.Id);

            var studentUpdated = studentOriginallyExisted ?
                _studentRepository.Update(student) :
                _studentRepository.Add(student);

            await _studentRepository.UnitOfWork
                .SaveEntitiesAsync(cancellationToken);

            var applicationStatusChangedTosubmittedIntegrationEvent = new ApplicationStatusChangedToSubmittedIntegrationEvent(applicationStartedEvent.Application.Id, applicationStartedEvent.Application.ApplicationStatus.Name, student.UserName);
            await _applyingIntegrationEventService.AddAndSaveEventAsync(applicationStatusChangedTosubmittedIntegrationEvent);
            _logger.CreateLogger<ValidateOrAddStudentAggregateWhenApplicationStartedDomainEventHandler>()
                .LogTrace("Student {StudentId} and related payment method were validated or updated for applicationId: {ApplicationId}.",
                    studentUpdated.Id, applicationStartedEvent.Application.Id);
        }
    }
}
