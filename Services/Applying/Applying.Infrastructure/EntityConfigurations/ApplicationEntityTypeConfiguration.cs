using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.ApplicationAggregate;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;
using System;

namespace Applying.Infrastructure.EntityConfigurations
{
    class ApplicationEntityTypeConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> applicationConfiguration)
        {
            applicationConfiguration.ToTable("applications", ApplyingContext.DEFAULT_SCHEMA);

            applicationConfiguration.HasKey(a => a.Id);

            applicationConfiguration.Ignore(a => a.DomainEvents);

            applicationConfiguration.Property(a => a.Id)
                .UseHiLo("applicationseq", ApplyingContext.DEFAULT_SCHEMA);

            applicationConfiguration
                .OwnsOne(o => o.Profile, a =>
                {
                    a.Property<int>("ApplicationId")
                    .UseHiLo("applicationseq", ApplyingContext.DEFAULT_SCHEMA);
                    a.WithOwner();
                });

            applicationConfiguration
                .Property<int?>("_studentId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("StudentId")
                .IsRequired(false);

            applicationConfiguration
                .Property<DateTime>("_applicationDate")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ApplicationDate")
                .IsRequired();

            applicationConfiguration
                .Property<int>("_applicationStatusId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("ApplicationStatusId")
                .IsRequired();

            applicationConfiguration
                .Property<int?>("_paymentMethodId")
                .UsePropertyAccessMode(PropertyAccessMode.Field)
                .HasColumnName("PaymentMethodId")
                .IsRequired(false);

            applicationConfiguration.Property<string>("Description").IsRequired(false);

            var navigation = applicationConfiguration.Metadata.FindNavigation(nameof(Application.ApplicationItems));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);

            applicationConfiguration.HasOne<PaymentMethod>()
                .WithMany()
                .HasForeignKey("_paymentMethodId")
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            applicationConfiguration.HasOne<Student>()
                .WithMany()
                .IsRequired(false)
                .HasForeignKey("_studentId");

            applicationConfiguration.HasOne(a => a.ApplicationStatus)
                .WithMany()
                .HasForeignKey("_applicationStatusId");
        }
    }
}
