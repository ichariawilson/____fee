using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipLocationEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipLocation>
    {
        public void Configure(EntityTypeBuilder<ScholarshipLocation> builder)
        {
            builder.ToTable("ScholarshipLocation");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("scholarship_location_hilo")
               .IsRequired();

            builder.Property(sl => sl.Location)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
