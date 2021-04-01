using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Applying.Domain.AggregatesModel.StudentAggregate;
using Microsoft.Fee.Services.Applying.Infrastructure;

namespace Applying.Infrastructure.EntityConfigurations
{
    class StudentEntityTypeConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> studentConfiguration)
        {
            studentConfiguration.ToTable("students", ApplyingContext.DEFAULT_SCHEMA);

            studentConfiguration.HasKey(s => s.Id);

            studentConfiguration.Ignore(s => s.DomainEvents);

            studentConfiguration.Property(s => s.Id)
                .UseHiLo("studentseq", ApplyingContext.DEFAULT_SCHEMA);

            studentConfiguration.Property(s => s.IdentityGuid)
                .HasMaxLength(200)
                .IsRequired();

            studentConfiguration.HasIndex("IdentityGuid")
              .IsUnique(true);

            studentConfiguration.Property(s => s.UserName);

            studentConfiguration.HasMany(s => s.PaymentMethods)
               .WithOne()
               .HasForeignKey("StudentId")
               .OnDelete(DeleteBehavior.Cascade);

            var navigation = studentConfiguration.Metadata.FindNavigation(nameof(Student.PaymentMethods));

            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
