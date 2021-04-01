using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels;
using Microsoft.Fee.Services.Student.Identity.API.Repositories;

namespace Microsoft.Fee.Services.Student.Identity.API.Views.Account
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public List<ProfileDisplayViewModel> ProfileDisplayList { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var repo = new ProfilesRepository(_context);
            ProfileDisplayList = repo.GetProfiles();
            return Page();
        }
    }
}
