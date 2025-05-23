﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(i => i.Name)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(i => i.CurrencyCode)
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(i => i.Price).IsRequired();
        }
    }
}
