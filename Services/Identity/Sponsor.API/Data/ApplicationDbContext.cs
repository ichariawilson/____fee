using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Fee.Services.Sponsor.API.Data.EntityConfigurations;
using Microsoft.Fee.Services.Sponsor.API.Models;
using System;

namespace Microsoft.Fee.Services.Sponsor.API.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<EducationLevel> EducationLevels { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<School> Schools { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new EducationLevelEntityTypeConfiguration());
            builder.ApplyConfiguration(new LocationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ProfileEntityTypeConfiguration());
            builder.ApplyConfiguration(new SchoolEntityTypeConfiguration());
            //builder.Entity<ApplicationUser>()
            //    .HasOne(a => a.Profile)
            //    .WithOne(p => p.User)
            //    .HasForeignKey<Profile>(p => p.UserId);


            builder.Entity<EducationLevel>().HasData(
             new EducationLevel
             {
                 Id = "LP",
                 Level = "Lower Primary"
             },
             new EducationLevel
             {
                 Id = "UP",
                 Level = "Upper Primary"
             },
             new EducationLevel
             {
                 Id = "PM",
                 Level = "Primary"
             },
             new EducationLevel
             {
                 Id = "SN",
                 Level = "Secondary"
             },
             new EducationLevel
             {
                 Id = "VC",
                 Level = "Vocational"
             },
             new EducationLevel
             {
                 Id = "CL",
                 Level = "College"
             },
             new EducationLevel
             {
                 Id = "UV",
                 Level = "University"
             });

            builder.Entity<Location>().HasData(
             new Location
             {
                 Id = "001",
                 County = "Mombasa"
             }, new Location
             {
                 Id = "002",
                 County = "Kwale"
             }, new Location
             {
                 Id = "003",
                 County = "Kilifi"
             }, new Location
             {
                 Id = "004",
                 County = "Tana River"
             }, new Location
             {
                 Id = "005",
                 County = "Lamu"
             }, new Location
             {
                 Id = "006",
                 County = "Taita/Taveta"
             }, new Location
             {
                 Id = "007",
                 County = "Garissa"
             }, new Location
             {
                 Id = "008",
                 County = "Wajir"
             }, new Location
             {
                 Id = "009",
                 County = "Mandera"
             }, new Location
             {
                 Id = "010",
                 County = "Marsabit"
             }, new Location
             {
                 Id = "011",
                 County = "Isiolo"
             }, new Location
             {
                 Id = "012",
                 County = "Meru"
             }, new Location
             {
                 Id = "013",
                 County = "Tharaka-Nithi"
             }, new Location
             {
                 Id = "014",
                 County = "Embu"
             });

            builder.Entity<School>().HasData(
             new School
             {
                 Id = "HS",
                 Name = "How in School",
                 EducationLevelId = "PM",
                 LocationId = "001"
             }, new School
             {
                 Id = "WS",
                 Name = "Where in School",
                 EducationLevelId = "SN",
                 LocationId = "013"
             }, new School
             {
                 Id = "THS",
                 Name = "That School",
                 EducationLevelId = "VC",
                 LocationId = "008"
             }, new School
             {
                 Id = "WHS",
                 Name = "Which School",
                 EducationLevelId = "CL",
                 LocationId = "005"
             }, new School
             {
                 Id = "TIS",
                 Name = "This School",
                 EducationLevelId = "UV",
                 LocationId = "005"
             });

            builder.Entity<Profile>().HasData(
             new Profile
             {
                 Id = Guid.NewGuid(),
                 //ProfilePicture = "a6ef4b55-f275-4b26-9e74-3de494cb5a08_125467014_419121702429319_4662892492889507213_n.jpg",
                 DateofBirth = DateTime.Parse("2005-09-01"),
                 EducationLevelId = "PM",
                 LocationId = "010",
                 SchoolId = "HS"
             }, 
             new Profile
             {
                 Id = Guid.NewGuid(),
                 //ProfilePicture = "a6ef4b55-f275-4b26-9e74-3de494cb5a08_125467014_419121702429319_4662892492889507213_n.jpg",
                 DateofBirth = DateTime.Parse("1985-09-01"),
                 EducationLevelId = "UV",
                 LocationId = "014",
                 SchoolId = "TIS"
             });
        }
    }
}
