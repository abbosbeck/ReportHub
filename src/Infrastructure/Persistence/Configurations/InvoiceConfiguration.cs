using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration(IClientRequest clientRequest) : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasQueryFilter(i => !i.IsDeleted);
        builder.HasQueryFilter(i => i.ClientId == clientRequest.ClientId);

        builder.Property(i => i.InvoiceNumber)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(i => i.CurrencyCode)
            .HasMaxLength(40)
            .IsRequired();

        builder.Property(i => i.DueDate).IsRequired();

        builder.Property(i => i.Amount).IsRequired();

        builder.Property(i => i.IsDeleted).HasDefaultValue(false);
    }
}
