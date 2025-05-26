using Application.Common.Interfaces.Authorization;
using Domain.Entities;
using Infrastructure.Persistence.Extensions;
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

    public DbSet<Plan> Plans { get; set; }

    public DbSet<PlanItem> PlanItems { get; set; }

    public DbSet<ReportSchedule> ReportSchedules { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        ApplyQueryFilters();

        IgnoreUnusedIdentityTables();

        void ApplyQueryFilters()
        {
            builder.Entity<Client>(entity => entity
                .HasQueryFilter(t => !t.IsDeleted));

            builder.Entity<Customer>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<Customer>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<Invoice>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<Item>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<Plan>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<ReportSchedule>(entity => entity
                .HasQueryFilter(t =>
                    t.ClientId == clientProvider.ClientId &&
                    !t.IsDeleted));

            builder.Entity<SystemRole>(entity => entity
                .HasQueryFilter(t => !t.IsDeleted));

            builder.Entity<SystemRoleAssignment>(entity => entity
                .HasQueryFilter(t => !t.IsDeleted));

            builder.Entity<User>(entity => entity
                .HasQueryFilter(t => !t.IsDeleted));
        }

        void IgnoreUnusedIdentityTables()
        {
            builder.Ignore<IdentityUserClaim<Guid>>();
            builder.Ignore<IdentityRoleClaim<Guid>>();
            builder.Ignore<IdentityUserToken<Guid>>();
            builder.Ignore<IdentityUserLogin<Guid>>();
        }
    }
}