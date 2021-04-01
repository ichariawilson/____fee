using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models
{
    public enum PaymentType
    {
        [Display(Name = "M-Pesa")]
        MPesa
    }
}
