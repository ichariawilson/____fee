using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipFeeStructureEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipFeeStructure>
    {
        public void Configure(EntityTypeBuilder<ScholarshipFeeStructure> builder)
        {
            builder.ToTable("ScholarshipFeeStructure");

            builder.Property(si => si.Id)
                .UseHiLo("scholarship_feestructure_hilo")
                .IsRequired();

            builder.Property(si => si.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(si => si.Fee)
                .IsRequired(true);

            builder.HasOne(si => si.ScholarshipCurrency)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipCurrencyId);

            builder.HasOne(si => si.ScholarshipDuration)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipDurationId);

            builder.HasOne(si => si.ScholarshipEducationLevel)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipEducationLevelId);

            builder.HasOne(si => si.ScholarshipCourse)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipCourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(si => si.ScholarshipSchool)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipSchoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
