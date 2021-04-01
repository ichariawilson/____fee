using Microsoft.Fee.Services.Applying.Domain.Seedwork;
using Applying.Domain.Exceptions;
using System;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate
{
    public class PaymentMethod : Entity
    {
        private int _paymentTypeId;

        public PaymentType PaymentType { get; private set; }

        protected PaymentMethod() { }

        public PaymentMethod(int paymentTypeId)
        {
            _paymentTypeId = paymentTypeId;
        }

        public bool IsEqualTo(int paymentTypeId)
        {
            return _paymentTypeId == paymentTypeId;
        }
    }
}
