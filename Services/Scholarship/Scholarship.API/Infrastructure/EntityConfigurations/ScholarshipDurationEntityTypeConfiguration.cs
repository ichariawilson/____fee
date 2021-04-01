using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipDurationEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipDuration>
    {
        public void Configure(EntityTypeBuilder<ScholarshipDuration> builder)
        {
            builder.ToTable("ScholarshipDuration");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("scholarship_duration_hilo")
               .IsRequired();

            builder.Property(sd => sd.Duration)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
