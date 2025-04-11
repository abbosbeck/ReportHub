using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class SystemRoleAssignmentConfiguration : IEntityTypeConfiguration<SystemRoleAssignment>
    {
        public void Configure(EntityTypeBuilder<SystemRoleAssignment> builder)
        {
            builder.ToTable("SystemRoleAssignments");

            builder.Ignore(x => x.User);
            builder.Ignore(x => x.Role);
        }
    }
}
