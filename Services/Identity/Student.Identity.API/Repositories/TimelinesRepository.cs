using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using Microsoft.Fee.Services.Student.Identity.API.Models;
using Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Student.Identity.API.Repositories
{
    public class TimelinesRepository
    {
        private readonly ApplicationDbContext _context;

        public TimelinesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<TimelineDisplayViewModel> GetTimelines()
        {
            List<Timeline> timelines = new List<Timeline>();
            timelines = _context.Timelines.AsNoTracking()
                .Include(x => x.School)
                .ToList();

            if (timelines != null)
            {
                List<TimelineDisplayViewModel> timelinesDisplay = new List<TimelineDisplayViewModel>();
                foreach (var x in timelines)
                {
                    var timelineDisplay = new TimelineDisplayViewModel()
                    {
                        TimelineId = x.Id,
                        Description = x.Description,
                        Period = x.Period,
                        School = x.School.Name

                    };
                    timelinesDisplay.Add(timelineDisplay);
                }
                return timelinesDisplay;
            }
            return null;
        }

        public TimelineViewModel CreateTimeline()
        {
            var sRepo = new SchoolsRepository(_context);
            var timeline = new TimelineViewModel()
            {
                TimelineId = Guid.NewGuid().ToString(),
                Schools = sRepo.GetSchools()
            };
            return timeline;
        }

        public bool SaveTimeline(TimelineViewModel timelineedit)
        {
            if (timelineedit != null)
            {
                if (Guid.TryParse(timelineedit.TimelineId, out Guid newGuid))
                {
                    var timeline = new Timeline()
                    {
                        Id = newGuid,
                        Description = timelineedit.Description,
                        Period = timelineedit.Period,
                        SchoolId = timelineedit.SchoolId
                    };
                    timeline.School = _context.Schools.Find(timelineedit.SchoolId);

                    _context.Timelines.Add(timeline);
                    _context.SaveChanges();
                    return true;
                }
            }
            // Return false if timelineedit == null of TimelineId is not guid
            return false;
        }
    }
}
