namespace Applying.Domain.Events
{
    using MediatR;

    /// <summary>
    /// Event used when the application slot items are confirmed
    /// </summary>
    public class ApplicationStatusChangedToSlotConfirmedDomainEvent
        : INotification
    {
        public int ApplicationId { get; }

        public ApplicationStatusChangedToSlotConfirmedDomainEvent(int applicationId)
            => ApplicationId = applicationId;
    }
}