using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;

namespace Applying.Domain.Events
{
    /// <summary>
    /// Event used when an application is created
    /// </summary>
    public class ApplicationStartedDomainEvent : INotification
    {
        public string UserId { get; }
        public string UserName { get; }
        public int PaymentTypeId { get; }
        public Application Application { get; }

        public ApplicationStartedDomainEvent(Application application, string userId, string userName, int paymentTypeId)
        {
            Application = application;
            UserId = userId;
            UserName = userName;
            PaymentTypeId = paymentTypeId;
        }
    }
}
