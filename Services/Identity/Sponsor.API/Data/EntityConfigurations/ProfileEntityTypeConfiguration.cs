using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Sponsor.API.Models;

namespace Microsoft.Fee.Services.Sponsor.API.Data.EntityConfigurations
{
    class ProfileEntityTypeConfiguration : IEntityTypeConfiguration<Profile>
    {
        public void Configure(EntityTypeBuilder<Profile> builder)
        {
            builder.ToTable("Profile");

            builder.HasKey(ci => ci.Id);

            //builder.Property(s => s.ProfilePicture)
            //    .IsRequired();

            builder.Property(s => s.DateofBirth)
                .IsRequired();

            builder.HasOne(si => si.School)
                .WithMany()
                .HasForeignKey(si => si.SchoolId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
