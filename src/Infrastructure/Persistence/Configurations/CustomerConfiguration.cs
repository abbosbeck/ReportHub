using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CustomerConfiguration(IClientRequest clientRequest) : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder.HasQueryFilter(c => c.ClientId == clientRequest.ClientId);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.CountryCode)
            .HasMaxLength(10)
            .IsRequired();
    }
}
