using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ClientRoleAssignmentConfiguration : IEntityTypeConfiguration<ClientRoleAssignment>
{
    public void Configure(EntityTypeBuilder<ClientRoleAssignment> builder)
    {
        builder.HasKey(cr => new { cr.UserId, cr.ClientId, cr.ClientRoleId });
    }
}
