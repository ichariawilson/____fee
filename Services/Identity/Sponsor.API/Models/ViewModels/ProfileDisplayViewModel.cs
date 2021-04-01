using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.Sponsor.API.Models.ViewModels
{
    public class ProfileDisplayViewModel
    {
        [Display(Name = "Profile Id")]
        public Guid ProfileId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateofBirth { get; set; }

        public string Level { get; set; }

        public string County { get; set; }

        public string School { get; set; }
    }
}
