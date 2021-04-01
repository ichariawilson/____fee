namespace Applying.Domain.Events
{
    using MediatR;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
    using System.Collections.Generic;

    /// <summary>
    /// Event used when the application is paid
    /// </summary>
    public class ApplicationStatusChangedToPaidDomainEvent : INotification
    {
        public int ApplicationId { get; }
        public IEnumerable<ApplicationItem> ApplicationItems { get; }

        public ApplicationStatusChangedToPaidDomainEvent(int applicationId, IEnumerable<ApplicationItem> applicationItems)
        {
            ApplicationId = applicationId;
            ApplicationItems = applicationItems;
        }
    }
}