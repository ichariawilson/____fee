using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Data;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Repositories
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
                .Include(x => x.Location)
                .ToList();

            if (profiles != null)
            {
                List<ProfileDisplayViewModel> profilesDisplay = new List<ProfileDisplayViewModel>();
                foreach (var x in profiles)
                {
                    var profileDisplay = new ProfileDisplayViewModel()
                    {
                        ProfileId = x.Id,
                        WebURL = x.WebURL,
                        Level = x.EducationLevel.Level,
                        County = x.Location.County

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
            var lRepo = new LocationsRepository(_context);
            var profile = new ProfileViewModel()
            {
                ProfileId = Guid.NewGuid().ToString(),
                EducationLevels = eRepo.GetEducationLevels(),
                Locations = lRepo.GetLocations()
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
                        WebURL = profileedit.WebURL,
                        EducationLevelId = profileedit.EducationLevelId,
                        LocationId = profileedit.LocationId
                    };
                    profile.Location = _context.Locations.Find(profileedit.LocationId);

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
