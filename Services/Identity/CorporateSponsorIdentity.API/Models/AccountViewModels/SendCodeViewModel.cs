using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.AccountViewModels
{
    public record SendCodeViewModel
    {
        public string SelectedProvider { get; init; }

        public ICollection<SelectListItem> Providers { get; init; }

        public string ReturnUrl { get; init; }

        public bool RememberMe { get; init; }
    }
}
