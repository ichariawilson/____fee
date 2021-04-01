using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;

namespace Applying.Infrastructure.EntityConfigurations
{
    class PaymentMethodEntityTypeConfiguration
        : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> paymentConfiguration)
        {
            paymentConfiguration.ToTable("paymentmethods", ApplyingContext.DEFAULT_SCHEMA);

            paymentConfiguration.HasKey(pm => pm.Id);

            paymentConfiguration.Ignore(pm => pm.DomainEvents);

            paymentConfiguration.Property(pm => pm.Id)
                .UseHiLo("paymentseq", ApplyingContext.DEFAULT_SCHEMA);

            paymentConfiguration.Property<int>("StudentId")
                .IsRequired();

            paymentConfiguration
                .Property<int>("_paymentTypeId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PaymentTypeId")
                .IsRequired();

            paymentConfiguration.HasOne(p => p.PaymentType)
                .WithMany()
                .HasForeignKey("_paymentTypeId");
        }
    }
}
