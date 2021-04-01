using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Student.Identity.API.Data;
using Microsoft.Fee.Services.Student.Identity.API.Models;
using Microsoft.Fee.Services.Student.Identity.API.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.Student.Identity.API.Repositories
{
    public class ProfilesRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfilesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ProfileDisplayViewModel> GetProfiles()
        {
            List<Profile> profiles = new List<Profile>();
            profiles = _context.Profiles.AsNoTracking()
                .Include(x => x.EducationLevel)
                .Include(x => x.Hobby)
                .Include(x => x.Location)
                .Include(x => x.School)
                .ToList();

            if (profiles != null)
            {
                List<ProfileDisplayViewModel> profilesDisplay = new List<ProfileDisplayViewModel>();
                foreach (var x in profiles)
                {
                    var profileDisplay = new ProfileDisplayViewModel()
                    {
                        ProfileId = x.Id,
                        DateofBirth = x.DateofBirth,
                        Level = x.EducationLevel.Level,
                        PaymentType = x.PaymentType,
                        Hobby = x.Hobby.Name,
                        County = x.Location.County,
                        School = x.School.Name

                    };
                    profilesDisplay.Add(profileDisplay);
                }
                return profilesDisplay;
            }
            return null;
        }

        public ProfileViewModel CreateProfile()
        {
            var eRepo = new EducationLevelsRepository(_context);
            var hRepo = new HobbiesRepository(_context);
            var lRepo = new LocationsRepository(_context);
            var sRepo = new SchoolsRepository(_context);
            var profile = new ProfileViewModel()
            {
                ProfileId = Guid.NewGuid().ToString(),
                EducationLevels = eRepo.GetEducationLevels(),
                Hobbies = hRepo.GetHobbies(),
                Locations = lRepo.GetLocations(),
                Schools = sRepo.GetSchools()
            };
            return profile;
        }

        public bool SaveProfile(ProfileViewModel profileedit)
        {
            if (profileedit != null)
            {
                if (Guid.TryParse(profileedit.ProfileId, out Guid newGuid))
                {
                    var profile = new Profile()
                    {
                        Id = newGuid,
                        DateofBirth = profileedit.DateofBirth,
                        PaymentType = profileedit.PaymentType,
                        EducationLevelId = profileedit.EducationLevelId,
                        HobbyId = profileedit.HobbyId,
                        LocationId = profileedit.LocationId,
                        SchoolId = profileedit.SchoolId
                    };
                    profile.EducationLevel = _context.EducationLevels.Find(profileedit.EducationLevelId);
                    profile.Hobby = _context.Hobbies.Find(profileedit.HobbyId);
                    profile.Location = _context.Locations.Find(profileedit.LocationId);
                    profile.School = _context.Schools.Find(profileedit.SchoolId);

                    _context.Profiles.Add(profile);
                    _context.SaveChanges();
                    return true;
                }
            }
            // Return false if profileedit == null of ProfileId is not guid
            return false;
        }
    }
}
