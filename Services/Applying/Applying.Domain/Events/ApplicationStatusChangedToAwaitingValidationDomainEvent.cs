namespace Applying.Domain.Events
{
    using MediatR;
    using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
    using System.Collections.Generic;

    /// <summary>
    /// Event used when the grace period application is confirmed
    /// </summary>
    public class ApplicationStatusChangedToAwaitingValidationDomainEvent : INotification
    {
        public int ApplicationId { get; }
        public IEnumerable<ApplicationItem> ApplicationItems { get; }

        public ApplicationStatusChangedToAwaitingValidationDomainEvent(int applicationId,
            IEnumerable<ApplicationItem> applicationItems)
        {
            ApplicationId = applicationId;
            ApplicationItems = applicationItems;
        }
    }
}