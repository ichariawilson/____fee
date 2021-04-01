using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Sponsor.API.Data;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Sponsor.API.Repositories
{
    public class LocationsRepository
    {
        private readonly ApplicationDbContext _context;

        public LocationsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetLocations()
        {
            List<SelectListItem> locations = _context.Locations.AsNoTracking()
                .OrderBy(n => n.County)
                .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Id.ToString(),
                        Text = n.County
                    }).ToList();
            var locationtip = new SelectListItem()
            {
                Value = null,
                Text = "--- select location ---"
            };
            locations.Insert(0, locationtip);
            return new SelectList(locations, "Value", "Text");
        }
    }
}
