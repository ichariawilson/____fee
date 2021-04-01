using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Scholarship.API.Model;

namespace Microsoft.Fee.Services.Scholarship.API.Infrastructure.EntityConfigurations
{
    class ScholarshipCurrencyEntityTypeConfiguration
        : IEntityTypeConfiguration<ScholarshipCurrency>
    {
        public void Configure(EntityTypeBuilder<ScholarshipCurrency> builder)
        {
            builder.ToTable("ScholarshipCurrency");

            builder.HasKey(ci => ci.Id);

            builder.Property(ci => ci.Id)
               .UseHiLo("scholarship_currency_hilo")
               .IsRequired();

            builder.Property(sc => sc.Symbol)
                .IsRequired()
                .HasMaxLength(3);

            builder.Property(sc => sc.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(sc => sc.Currency)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
