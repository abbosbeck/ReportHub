using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IClientIdProvider clientProvider)
    : IdentityDbContext<
        User,
        SystemRole,
        Guid,
        IdentityUserClaim<Guid>,
        SystemRoleAssignment,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>>(options),
    IDataProtectionKeyContext
{
    public DbSet<Client> Clients { get; set; }

    public DbSet<ClientRole> ClientRoles { get; set; }

    public DbSet<ClientRoleAssignment> ClientRoleAssignments { get; set; }

    public DbSet<Invoice> Invoices { get; set; }

    public DbSet<Item> Items { get; set; }

    public DbSet<Customer> Customers { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        builder.Entity<Customer>(entity => entity
            .HasQueryFilter(c => c.ClientId == clientProvider.ClientId));

        IgnoreUnusedIdentityTables(builder);

        builder.Entity<SystemRole>().ToTable("SystemRoles");
    }

    private static void IgnoreUnusedIdentityTables(ModelBuilder builder)
    {
        builder.Ignore<IdentityUserClaim<Guid>>();
        builder.Ignore<IdentityRoleClaim<Guid>>();
        builder.Ignore<IdentityUserToken<Guid>>();
        builder.Ignore<IdentityUserLogin<Guid>>();
    }
}