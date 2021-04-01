using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Extensions.Logging;
using Applying.Domain.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Applying.API.Application.DomainEventHandlers.StudentAndPaymentMethodVerified
{
    public class UpdateApplicationWhenStudentAndPaymentMethodVerifiedDomainEventHandler
                   : INotificationHandler<StudentAndPaymentMethodVerifiedDomainEvent>
    {
        private readonly IApplicationRepository _applicationRepository;
        private readonly ILoggerFactory _logger;

        public UpdateApplicationWhenStudentAndPaymentMethodVerifiedDomainEventHandler(
            IApplicationRepository applicationRepository, ILoggerFactory logger)
        {
            _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Domain Logic comment:
        // When the Student and Student's payment method have been created or verified that they existed, 
        // then we can update the original Application with the StudentId and PaymentId (foreign keys)
        public async Task Handle(StudentAndPaymentMethodVerifiedDomainEvent studentPaymentMethodVerifiedEvent, CancellationToken cancellationToken)
        {
            var applicationToUpdate = await _applicationRepository.GetAsync(studentPaymentMethodVerifiedEvent.ApplicationId);
            applicationToUpdate.SetStudentId(studentPaymentMethodVerifiedEvent.Student.Id);
            applicationToUpdate.SetPaymentId(studentPaymentMethodVerifiedEvent.Payment.Id);

            _logger.CreateLogger<UpdateApplicationWhenStudentAndPaymentMethodVerifiedDomainEventHandler>()
                .LogTrace("Application with Id: {ApplicationId} has been successfully updated with a payment method {PaymentMethod} ({Id})",
                    studentPaymentMethodVerifiedEvent.ApplicationId, nameof(studentPaymentMethodVerifiedEvent.Payment), studentPaymentMethodVerifiedEvent.Payment.Id);
        }
    }
}
