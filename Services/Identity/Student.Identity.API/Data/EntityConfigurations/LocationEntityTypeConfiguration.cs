using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Student.Identity.API.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Data.EntityConfigurations
{
    class LocationEntityTypeConfiguration
        : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Location");

            builder.HasKey(ci => ci.Id);

            builder.Property(l => l.County)
                .IsRequired()
                .HasMaxLength(32);
        }
    }
}
