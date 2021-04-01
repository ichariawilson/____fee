using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;

namespace Applying.Infrastructure.EntityConfigurations
{
    class ApplicationStatusEntityTypeConfiguration
        : IEntityTypeConfiguration<ApplicationStatus>
    {
        public void Configure(EntityTypeBuilder<ApplicationStatus> applicationStatusConfiguration)
        {
            applicationStatusConfiguration.ToTable("applicationstatus", ApplyingContext.DEFAULT_SCHEMA);

            applicationStatusConfiguration.HasKey(a => a.Id);

            applicationStatusConfiguration.Property(a => a.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            applicationStatusConfiguration.Property(a => a.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
