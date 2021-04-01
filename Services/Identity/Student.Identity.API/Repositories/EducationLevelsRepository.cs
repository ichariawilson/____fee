using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Student.Identity.API.Repositories
{
    public class EducationLevelsRepository
    {
        private readonly ApplicationDbContext _context;

        public EducationLevelsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetEducationLevels()
        {
            List<SelectListItem> educationlevels = _context.EducationLevels.AsNoTracking()
                .OrderBy(n => n.Level)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.Level
                    }).ToList();
            var educationleveltip = new SelectListItem()
            {
                Value = null,
                Text = "--- select education level ---"
            };
            educationlevels.Insert(0, educationleveltip);
            return new SelectList(educationlevels, "Value", "Text");
        }
    }
}
