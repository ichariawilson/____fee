namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate
{
    using global::Applying.Domain.Exceptions;
    using Microsoft.Fee.Services.Applying.Domain.SeedWork;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ApplicationStatus
        : Enumeration
    {
        public static ApplicationStatus Submitted = new ApplicationStatus(1, nameof(Submitted).ToLowerInvariant());
        public static ApplicationStatus AwaitingValidation = new ApplicationStatus(2, nameof(AwaitingValidation).ToLowerInvariant());
        public static ApplicationStatus SlotConfirmed = new ApplicationStatus(3, nameof(SlotConfirmed).ToLowerInvariant());
        public static ApplicationStatus Paid = new ApplicationStatus(4, nameof(Paid).ToLowerInvariant());
        public static ApplicationStatus Granted = new ApplicationStatus(5, nameof(Granted).ToLowerInvariant());
        public static ApplicationStatus Cancelled = new ApplicationStatus(6, nameof(Cancelled).ToLowerInvariant());

        public ApplicationStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<ApplicationStatus> List() =>
            new[] { Submitted, AwaitingValidation, SlotConfirmed, Paid, Granted, Cancelled };

        public static ApplicationStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new ApplyingDomainException($"Possible values for ApplicationStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static ApplicationStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new ApplyingDomainException($"Possible values for ApplicationStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
