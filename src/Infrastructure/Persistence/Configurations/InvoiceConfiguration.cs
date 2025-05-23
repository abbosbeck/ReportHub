﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.Property(i => i.InvoiceNumber).UseIdentityColumn();

        builder.Property(i => i.CurrencyCode)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(i => i.DueDate).IsRequired();

        builder.Property(i => i.Amount).IsRequired();

        builder.Property(i => i.IsDeleted).HasDefaultValue(false);
    }
}
