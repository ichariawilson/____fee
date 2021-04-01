using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;

namespace Applying.Domain.Events
{
    public class StudentAndPaymentMethodVerifiedDomainEvent : INotification
    {
        public Student Student { get; private set; }
        public PaymentMethod Payment { get; private set; }
        public int ApplicationId { get; private set; }

        public StudentAndPaymentMethodVerifiedDomainEvent(Student student, PaymentMethod payment, int applicationId)
        {
            Student = student;
            Payment = payment;
            ApplicationId = applicationId;
        }
    }
}
