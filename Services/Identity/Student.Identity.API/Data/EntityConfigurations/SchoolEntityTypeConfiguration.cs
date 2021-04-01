﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Fee.Services.Student.Identity.API.Models;

namespace Microsoft.Fee.Services.Student.Identity.API.Data.EntityConfigurations
{
    class SchoolEntityTypeConfiguration
        : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.ToTable("School");

            builder.HasKey(ci => ci.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
