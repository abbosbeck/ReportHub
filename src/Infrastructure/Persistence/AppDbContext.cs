using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext : IdentityDbContext<
    User,
    SystemRole,
    Guid,
    IdentityUserClaim<Guid>,
    UserSystemRole,
    IdentityUserLogin<Guid>,
    IdentityRoleClaim<Guid>,
    IdentityUserToken<Guid>>
{
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Client> Clients { get; set; }

    public DbSet<ClientRole> ClientRoles { get; set; }

    public DbSet<UserClientRole> UserClientRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        IgnoreUnusedIdentityTables(builder);

        builder.Entity<SystemRole>().ToTable("SystemRoles");
        builder.Entity<UserSystemRole>().ToTable("UserSystemRoles");
    }

    private static void IgnoreUnusedIdentityTables(ModelBuilder builder)
    {
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
    }
}