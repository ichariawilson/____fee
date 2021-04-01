using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Student.Identity.API.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Data.EntityConfigurations
{
    class HobbyEntityTypeConfiguration
        : IEntityTypeConfiguration<Hobby>
    {
        public void Configure(EntityTypeBuilder<Hobby> builder)
        {
            builder.ToTable("Hobby");

            builder.HasKey(ci => ci.Id);

            builder.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(48);
        }
    }
}
