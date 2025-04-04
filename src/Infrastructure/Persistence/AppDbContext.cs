using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User, UserRole, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);

        base.OnModelCreating(builder);

        builder.Entity<User>()
            .Ignore(u => u.AccessFailedCount)
            .Ignore(u => u.EmailConfirmed)
            .Ignore(u => u.UserName)
            .Ignore(u => u.Email)
            .Ignore(u => u.LockoutEnabled)
            .Ignore(u => u.TwoFactorEnabled)
            .Ignore(u => u.LockoutEnd)
            .Ignore(u => u.NormalizedEmail)
            .Ignore(u => u.NormalizedUserName);
    }
}