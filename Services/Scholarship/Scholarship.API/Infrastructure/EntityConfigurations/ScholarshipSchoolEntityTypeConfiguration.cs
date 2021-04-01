using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipSchoolEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipSchool>
    {
        public void Configure(EntityTypeBuilder<ScholarshipSchool> builder)
        {
            builder.ToTable("ScholarshipSchool");

            builder.Property(si => si.Id)
                .UseHiLo("scholarship_school_hilo")
                .IsRequired();

            builder.Property(si => si.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(si => si.EmailAddress)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(si => si.PhoneNumber)
                .IsRequired(true)
                .HasMaxLength(10)
                .IsRequired(true);

            builder.HasOne(si => si.ScholarshipLocation)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipLocationId);
        }
    }
}
