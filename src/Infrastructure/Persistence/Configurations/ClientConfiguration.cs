using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class ClientConfiguration(IClientRequest clientRequest) : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.HasQueryFilter(c => !c.IsDeleted);
        builder.HasQueryFilter(c => c.Id == clientRequest.ClientId);

        builder.Property(c => c.Name)
            .HasMaxLength(200)
            .IsRequired();
    }
}
