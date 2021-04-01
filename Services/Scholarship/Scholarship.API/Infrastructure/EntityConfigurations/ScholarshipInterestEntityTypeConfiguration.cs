using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipInterestEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipInterest>
    {
        public void Configure(EntityTypeBuilder<ScholarshipInterest> builder)
        {
            builder.ToTable("ScholarshipInterest");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("scholarship_interest_hilo")
               .IsRequired();

            builder.Property(si => si.Interest)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
