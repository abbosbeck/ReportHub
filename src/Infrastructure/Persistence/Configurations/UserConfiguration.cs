using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted);

        builder.Property(u => u.FirstName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(u => u.LastName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Ignore(u => u.AccessFailedCount)
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.UserName)
            .Ignore(u => u.Email)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.NormalizedEmail)
            .Ignore(u => u.NormalizedUserName)
            .Ignore(u => u.SecurityStamp)
            .Ignore(u => u.PhoneNumberConfirmed);
    }
}
