using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels
{
    public class ProfileDisplayViewModel
    {
        public Guid ProfileId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateofBirth { get; set; }

        public PaymentType PaymentType { get; set; }

        public string Hobby { get; set; }

        public string Level { get; set; }

        public string County { get; set; }

        public string School { get; set; }
    }
}
