using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using Applying.Domain.Events;
using Applying.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate
{
    public class Application : Entity, IAggregateRoot
    {
        private DateTime _applicationDate;

        public Profile Profile { get; private set; }

        public int? GetStudentId => _studentId;
        private int? _studentId;

        public ApplicationStatus ApplicationStatus { get; private set; }
        private int _applicationStatusId;

        private string _description;

        // Draft applications have this set to true. Currently we don't check anywhere the draft status of an Application, but we could do it if needed
        private bool _isDraft;

        // DDD Patterns comment
        // Using a private collection field, better for DDD Aggregate's encapsulation
        // so ApplicationItems cannot be added from "outside the AggregateRoot" directly to the collection,
        // but only through the method ApplicationAggrergateRoot.AddApplicationItem() which includes behaviour.
        private readonly List<ApplicationItem> _applicationItems;
        public IReadOnlyCollection<ApplicationItem> ApplicationItems => _applicationItems;

        private int? _paymentMethodId;

        public static Application NewDraft()
        {
            var application = new Application();
            application._isDraft = true;
            return application;
        }

        protected Application()
        {
            _applicationItems = new List<ApplicationItem>();
            _isDraft = false;
        }

        public Application(string userId, string userName, Profile profile, int paymentTypeId, int? studentId = null, int? paymentMethodId = null) : this()
        {
            _studentId = studentId;
            _paymentMethodId = paymentMethodId;
            _applicationStatusId = ApplicationStatus.Submitted.Id;
            _applicationDate = DateTime.UtcNow;
            Profile = profile;

            // Add the ApplicationStarterDomainEvent to the domain events collection 
            // to be raised/dispatched when comitting changes into the Database [ After DbContext.SaveChanges() ]
            AddApplicationStartedDomainEvent(userId, userName, paymentTypeId);
        }

        // DDD Patterns comment
        // This Application AggregateRoot's method "AddApplicationitem()" should be the only way to add Items to the Application,
        // so any behavior and validations are controlled by the AggregateRoot 
        // in application to maintain consistency between the whole Aggregate. 
        public void AddApplicationItem(int scholarshipItemId, string scholarshipItemName, decimal slotAmount, string pictureUrl, int slots = 1)
        {
            var existingApplicationForScholarshipItem = _applicationItems.Where(a => a.ScholarshipItemId == scholarshipItemId)
                .SingleOrDefault();

            if (existingApplicationForScholarshipItem != null)
            {
                existingApplicationForScholarshipItem.AddSlots(slots);
            }
            else
            {
                //add validated new application item

                var applicationItem = new ApplicationItem(scholarshipItemId, scholarshipItemName, slotAmount, pictureUrl, slots);
                _applicationItems.Add(applicationItem);
            }
        }

        public void SetPaymentId(int id)
        {
            _paymentMethodId = id;
        }

        public void SetStudentId(int id)
        {
            _studentId = id;
        }

        public void SetAwaitingValidationStatus()
        {
            if (_applicationStatusId == ApplicationStatus.Submitted.Id)
            {
                AddDomainEvent(new ApplicationStatusChangedToAwaitingValidationDomainEvent(Id, _applicationItems));
                _applicationStatusId = ApplicationStatus.AwaitingValidation.Id;
            }
        }

        public void SetSlotConfirmedStatus()
        {
            if (_applicationStatusId == ApplicationStatus.AwaitingValidation.Id)
            {
                AddDomainEvent(new ApplicationStatusChangedToSlotConfirmedDomainEvent(Id));

                _applicationStatusId = ApplicationStatus.SlotConfirmed.Id;
                _description = "All the items were confirmed with available slots.";
            }
        }

        public void SetPaidStatus()
        {
            if (_applicationStatusId == ApplicationStatus.SlotConfirmed.Id)
            {
                AddDomainEvent(new ApplicationStatusChangedToPaidDomainEvent(Id, ApplicationItems));

                _applicationStatusId = ApplicationStatus.Paid.Id;
                _description = "The payment was performed at a simulated M-Pesa account";
            }
        }

        public void SetGrantedStatus()
        {
            if (_applicationStatusId != ApplicationStatus.Paid.Id)
            {
                StatusChangeException(ApplicationStatus.Granted);
            }

            _applicationStatusId = ApplicationStatus.Granted.Id;
            _description = "The application was granted.";
            AddDomainEvent(new ApplicationGrantedDomainEvent(this));
        }

        public void SetCancelledStatus()
        {
            if (_applicationStatusId == ApplicationStatus.Paid.Id ||
                _applicationStatusId == ApplicationStatus.Granted.Id)
            {
                StatusChangeException(ApplicationStatus.Cancelled);
            }

            _applicationStatusId = ApplicationStatus.Cancelled.Id;
            _description = $"The application was cancelled.";
            AddDomainEvent(new ApplicationCancelledDomainEvent(this));
        }

        public void SetCancelledStatusWhenSlotIsRejected(IEnumerable<int> applicationSlotRejectedItems)
        {
            if (_applicationStatusId == ApplicationStatus.AwaitingValidation.Id)
            {
                _applicationStatusId = ApplicationStatus.Cancelled.Id;

                var itemsSlotRejectedScholarshipItemNames = ApplicationItems
                    .Where(c => applicationSlotRejectedItems.Contains(c.ScholarshipItemId))
                    .Select(c => c.GetApplicationItemScholarshipItemName());

                var itemsSlotRejectedDescription = string.Join(", ", itemsSlotRejectedScholarshipItemNames);
                _description = $"The scholarship items don't have slots: ({itemsSlotRejectedDescription}).";
            }
        }

        private void AddApplicationStartedDomainEvent(string userId, string userName, int paymentTypeId)
        {
            var applicationStartedDomainEvent = new ApplicationStartedDomainEvent(this, userId, userName, paymentTypeId);

            this.AddDomainEvent(applicationStartedDomainEvent);
        }

        private void StatusChangeException(ApplicationStatus applicationStatusToChange)
        {
            throw new ApplyingDomainException($"It's not possible to change the application status from {ApplicationStatus.Name} to {applicationStatusToChange.Name}.");
        }

        public decimal GetTotal()
        {
            return _applicationItems.Sum(a => a.GetSlots() * a.GetSlotAmount());
        }
    }
}

