using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.CorporateSponsorIdentity.API.Models;

namespace Microsoft.Fee.Services.CorporateSponsorIdentity.API.Data.EntityConfigurations
{
    class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profile");

            builder.HasKey(ci => ci.Id);

            builder.Property(s => s.WebURL)
                .IsRequired();
        }
    }
}
