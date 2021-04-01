using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using Applying.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate
{
    public class Student : Entity, IAggregateRoot
    {
        public string IdentityGuid { get; private set; }

        public string UserName { get; private set; }

        private List<PaymentMethod> _paymentMethods;

        public IEnumerable<PaymentMethod> PaymentMethods => _paymentMethods.AsReadOnly();

        protected Student()
        {
            _paymentMethods = new List<PaymentMethod>();
        }

        public Student(string identity, string name) : this()
        {
            IdentityGuid = !string.IsNullOrWhiteSpace(identity) ? identity : throw new ArgumentNullException(nameof(identity));
            UserName = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentNullException(nameof(name));
        }

        public PaymentMethod VerifyOrAddPaymentMethod( int paymentTypeId, int applicationId)
        {
            var existingPayment = _paymentMethods
                .SingleOrDefault(p => p.IsEqualTo(paymentTypeId));

            if (existingPayment != null)
            {
                AddDomainEvent(new StudentAndPaymentMethodVerifiedDomainEvent(this, existingPayment, applicationId));

                return existingPayment;
            }

            var payment = new PaymentMethod(paymentTypeId);

            _paymentMethods.Add(payment);

            AddDomainEvent(new StudentAndPaymentMethodVerifiedDomainEvent(this, payment, applicationId));

            return payment;
        }
    }
}
