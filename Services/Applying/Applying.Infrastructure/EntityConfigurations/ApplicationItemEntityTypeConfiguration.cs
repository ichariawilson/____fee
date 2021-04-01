using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;

namespace Applying.Infrastructure.EntityConfigurations
{
    class ApplicationItemEntityTypeConfiguration
        : IEntityTypeConfiguration<ApplicationItem>
    {
        public void Configure(EntityTypeBuilder<ApplicationItem> applicationItemConfiguration)
        {
            applicationItemConfiguration.ToTable("applicationitems", ApplyingContext.DEFAULT_SCHEMA);

            applicationItemConfiguration.HasKey(a => a.Id);

            applicationItemConfiguration.Ignore(s => s.DomainEvents);

            applicationItemConfiguration.Property(a => a.Id)
                .UseHiLo("applicationitemseq");

            applicationItemConfiguration.Property<int>("ApplicationId")
                .IsRequired();

            applicationItemConfiguration.Property<int>("ScholarshipItemId")
                .IsRequired();

            applicationItemConfiguration
                .Property<string>("_scholarshipItemName")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ScholarshipItemName")
                .IsRequired();

            applicationItemConfiguration
                .Property<decimal>("_slotAmount")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("SlotAmount")
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            applicationItemConfiguration
                .Property<int>("_slots")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("Slots")
                .IsRequired();

            applicationItemConfiguration
                .Property<string>("_pictureUrl")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PictureUrl")
                .IsRequired(false);
        }
    }
}
