using Microsoft.Fee.Services.Applying.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate
{
    public class Profile : ValueObject
    {
        public String IDNumber { get; private set; }

        [Column(TypeName = "decimal(18,4)")]
        public Decimal Request { get; private set; }

        public Profile() { }

        public Profile(string idNumber, decimal request)
        {
            IDNumber = idNumber;
            Request = request;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return IDNumber;
            yield return Request;
        }
    }
}
