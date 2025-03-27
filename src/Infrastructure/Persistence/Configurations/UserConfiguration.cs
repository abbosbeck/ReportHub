using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(200)
            .IsRequired();

        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
