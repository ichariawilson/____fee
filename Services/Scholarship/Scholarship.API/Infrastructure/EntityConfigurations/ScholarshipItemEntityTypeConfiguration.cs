using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipItemEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipItem>
    {
        public void Configure(EntityTypeBuilder<ScholarshipItem> builder)
        {
            builder.ToTable("Scholarship");

            builder.Property(si => si.Id)
                .UseHiLo("scholarship_hilo")
                .IsRequired();

            builder.Property(si => si.Name)
                .IsRequired(true)
                .HasMaxLength(50);

            builder.Property(si => si.Amount)
                .IsRequired(true);

            //builder.Property(si => si.CreateDate)
            //    .IsRequired(true);

            //builder.Property(si => si.Deadline)
                //.IsRequired(true);

            builder.Property(si => si.PictureFileName)
                .IsRequired(false);

            builder.Ignore(si => si.PictureUri);

            builder.HasOne(si => si.ScholarshipCurrency)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipCurrencyId);

            builder.HasOne(si => si.ScholarshipDuration)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipDurationId);

            builder.HasOne(si => si.ScholarshipEducationLevel)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipEducationLevelId);

            builder.HasOne(si => si.ScholarshipInterest)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipInterestId);

            builder.HasOne(si => si.ScholarshipLocation)
                .WithMany()
                .HasForeignKey(si => si.ScholarshipLocationId);
        }
    }
}
