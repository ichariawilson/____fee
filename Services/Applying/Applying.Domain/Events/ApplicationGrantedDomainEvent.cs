using MediatR;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;

namespace Applying.Domain.Events
{
    public class ApplicationGrantedDomainEvent : INotification
    {
        public Application Application { get; }

        public ApplicationGrantedDomainEvent(Application application)
        {
            Application = application;
        }
    }
}
