using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels;
using Microsoft.Fee.Services.Student.Identity.API.Repositories;

namespace Microsoft.Fee.Services.Student.Identity.API.Views.Account
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

        public IActionResult OnPostSchools_Locations()
        {
            MemoryStream stream = new MemoryStream();
            Request.Body.CopyToAsync(stream);
            stream.Position = 0;
            using StreamReader reader = new StreamReader(stream);
            string requestBody = reader.ReadToEnd();
            if (requestBody.Length > 0)
            {
                var repo = new SchoolsRepository(_context);

                IEnumerable<SelectListItem> schools = repo.GetSchoolsLocation(requestBody);
                return new JsonResult(schools);
            }
            return null;
        }
    }
}
