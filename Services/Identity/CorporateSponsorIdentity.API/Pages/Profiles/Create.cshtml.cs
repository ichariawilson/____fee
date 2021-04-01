using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Data;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Repositories;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Views.Account
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public ProfileViewModel ProfileViewModel { get; set; }

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var repo = new ProfilesRepository(_context);
            ProfileViewModel = repo.CreateProfile();
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var repo = new ProfilesRepository(_context);
                    bool saved = repo.SaveProfile(ProfileViewModel);
                    if (saved)
                    {
                        return RedirectToPage("Index");
                    }
                }
                // Handling model state errors is beyond the scope of the demo, so just throwing an ApplicationException when the ModelState is invalid
                // and rethrowing it in the catch block.
                throw new ApplicationException("Invalid model");
            }
            catch (ApplicationException ex)
            {
                Debug.Write(ex.Message);
                throw;
            }
        }
    }
}
