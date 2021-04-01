using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Student.Identity.API.Repositories
{
    public class SchoolsRepository
    {
        private readonly ApplicationDbContext _context;

        public SchoolsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetSchools()
        {
            List<SelectListItem> schools = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Value = null,
                    Text = " "
                }
            };
            return schools;
        }

        public IEnumerable<SelectListItem> GetSchoolsLocation(string locationId)
        {
            if (!String.IsNullOrWhiteSpace(locationId))
            {
                IEnumerable<SelectListItem> schools = _context.Schools.AsNoTracking()
                    .OrderBy(n => n.Name)
                    .Where(n => n.LocationId == locationId)
                    .Select(n =>
                        new SelectListItem
                        {
                            Value = n.Id.ToString(),
                            Text = n.Name
                        }).ToList();
                return new SelectList(schools, "Value", "Text");
            }
            return null;
        }
    }
}
