using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Sponsor.API.Models;

namespace Microsoft.Fee.Services.Sponsor.API.Data.EntityConfigurations
{
    class EducationLevelEntityTypeConfiguration
        : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            builder.ToTable("EducationLevel");

            builder.HasKey(ci => ci.Id);

            builder.Property(l => l.Level)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
