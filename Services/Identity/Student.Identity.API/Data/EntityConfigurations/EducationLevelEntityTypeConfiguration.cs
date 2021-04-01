using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Student.Identity.API.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Data.EntityConfigurations
{
    class EducationLevelEntityTypeConfiguration
        : IEntityTypeConfiguration<EducationLevel>
    {
        public void Configure(EntityTypeBuilder<EducationLevel> builder)
        {
            builder.ToTable("EducationLevel");

            builder.HasKey(ci => ci.Id);

            builder.Property(el => el.Level)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
