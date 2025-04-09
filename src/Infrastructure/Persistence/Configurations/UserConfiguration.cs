using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.Property(u => u.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .Ignore(u => u.AccessFailedCount)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.PhoneNumber)
            .Ignore(u => u.PhoneNumberConfirmed);
    }
}