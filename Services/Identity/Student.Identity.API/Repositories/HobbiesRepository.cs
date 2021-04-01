using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Student.Identity.API.Repositories
{
    public class HobbiesRepository
    {
        private readonly ApplicationDbContext _context;

        public HobbiesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetHobbies()
        {
            List<SelectListItem> hobbies = _context.Hobbies.AsNoTracking()
                .OrderBy(n => n.Name)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Name
                    }).ToList();
            var hobbytip = new SelectListItem()
            {
                Value = null,
                Text = "--- select hobby ---"
            };
            hobbies.Insert(0, hobbytip);
            return new SelectList(hobbies, "Value", "Text");
        }
    }
}
