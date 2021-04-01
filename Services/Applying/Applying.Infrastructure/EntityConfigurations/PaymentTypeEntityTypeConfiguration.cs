using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;

namespace Applying.Infrastructure.EntityConfigurations
{
    class PaymentTypeEntityTypeConfiguration
        : IEntityTypeConfiguration<PaymentType>
    {
        public void Configure(EntityTypeBuilder<PaymentType> paymentTypesConfiguration)
        {
            paymentTypesConfiguration.ToTable("paymenttypes", ApplyingContext.DEFAULT_SCHEMA);

            paymentTypesConfiguration.HasKey(pt => pt.Id);

            paymentTypesConfiguration.Property(pt => pt.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            paymentTypesConfiguration.Property(pt => pt.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
