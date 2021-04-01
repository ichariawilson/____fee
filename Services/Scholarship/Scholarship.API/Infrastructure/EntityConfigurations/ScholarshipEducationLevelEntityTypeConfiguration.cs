using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipEducationLevelEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipEducationLevel>
    {
        public void Configure(EntityTypeBuilder<ScholarshipEducationLevel> builder)
        {
            builder.ToTable("ScholarshipEducationLevel");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("scholarship_educationlevel_hilo")
               .IsRequired();

            builder.Property(se => se.EducationLevel)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
