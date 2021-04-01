using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Student.Identity.API.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Data.EntityConfigurations
{
    class TimelineEntityTypeConfiguration
        : IEntityTypeConfiguration<Timeline>
    {
        public void Configure(EntityTypeBuilder<Timeline> builder)
        {
            builder.ToTable("Timeline");

            builder.HasKey(ci => ci.Id);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(t => t.Period)
                .IsRequired();

            builder.HasOne(t => t.School)
                .WithMany()
                .HasForeignKey(t => t.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
