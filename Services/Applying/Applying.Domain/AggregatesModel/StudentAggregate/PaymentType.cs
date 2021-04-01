using Microsoft.Fee.Services.Applying.Domain.SeedWork;

namespace Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate
{
    /// <remarks> 
    /// Payment type class should be marked as abstract with protected constructor to encapsulate known enum types
    /// this is currently not possible as ApplyingringContextSeed uses this constructor to load paymentTypes from csv file
    /// </remarks>
    public class PaymentType : Enumeration
    {
        public static PaymentType MPesa = new PaymentType(1, "M-Pesa");

        public PaymentType(int id, string name) : base(id, name)
        {
        }
    }
}
