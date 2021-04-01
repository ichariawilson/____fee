namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure
{
    using EntityConfigurations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Model;

    public class ScholarshipContext : DbContext
    {
        public ScholarshipContext(DbContextOptions<ScholarshipContext> options) : base(options)
        {
        }
        public DbSet<ScholarshipItem> ScholarshipItems { get; set; }
        public DbSet<ScholarshipCourse> ScholarshipCourses { get; set; }
        public DbSet<ScholarshipCurrency> ScholarshipCurrencies { get; set; }
        public DbSet<ScholarshipDuration> ScholarshipDurations { get; set; }
        public DbSet<ScholarshipEducationLevel> ScholarshipEducationLevels { get; set; }
        public DbSet<ScholarshipFeeStructure> ScholarshipFeeStructures { get; set; }
        public DbSet<ScholarshipInterest> ScholarshipInterests { get; set; }
        public DbSet<ScholarshipLocation> ScholarshipLocations { get; set; }
        public DbSet<ScholarshipSchool> ScholarshipSchools { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ScholarshipCourseEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipCurrencyEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipDurationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipEducationLevelEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipFeeStructureEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipInterestEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipLocationEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipItemEntityTypeConfiguration());
            builder.ApplyConfiguration(new ScholarshipSchoolEntityTypeConfiguration());
        }
    }

    public class CatalogContextDesignFactory : IDesignTimeDbContextFactory<ScholarshipContext>
    {
        public ScholarshipContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ScholarshipContext>()
                .UseSqlServer("Server=.;Initial Catalog=Microsoft.Fee.Services.ScholarshipDb;Integrated Security=true");

            return new ScholarshipContext(optionsBuilder.Options);
        }
    }
}
