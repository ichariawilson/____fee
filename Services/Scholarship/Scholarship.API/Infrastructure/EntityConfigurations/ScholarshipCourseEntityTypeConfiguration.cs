using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipCourseEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipCourse>
    {
        public void Configure(EntityTypeBuilder<ScholarshipCourse> builder)
        {
            builder.ToTable("ScholarshipCourse");

            builder.Property(si => si.Id)
                .UseHiLo("scholarship_course_hilo")
                .IsRequired();

            builder.Property(si => si.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(si => si.Description)
                .IsRequired(true)
                .HasMaxLength(200);

            builder.Property(si => si.Fee)
                .IsRequired(true);

            builder.HasOne(si => si.ScholarshipDuration)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipDurationId);
        }
    }
}
