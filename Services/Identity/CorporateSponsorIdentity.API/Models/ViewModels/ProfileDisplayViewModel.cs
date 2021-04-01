using System;
using System.ComponentModel.DataAnnotations;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels
{
    public class ProfileDisplayViewModel
    {
        [Display(Name = "Profile Id")]
        public Guid ProfileId { get; set; }

        [Display(Name = "Web URL")]
        public string WebURL { get; set; }

        public string Level { get; set; }

        public string County { get; set; }
    }
}
