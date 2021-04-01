using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;

namespace Applying.Domain.Events
{
    public class ApplicationCancelledDomainEvent : INotification
    {
        public Application Application { get; }

        public ApplicationCancelledDomainEvent(Application application)
        {
            Application = application;
        }
    }
}
