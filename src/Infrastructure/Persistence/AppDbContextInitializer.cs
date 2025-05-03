using Application.Common.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence;

public static class InitializerExtensions
{
    public static async Task InitialiseDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var initializer = scope.ServiceProvider.GetRequiredService<AppDbContextInitializer>();

        await initializer.InitialiseAsync();

        await initializer.SeedAsync();
    }
}

public class AppDbContextInitializer(AppDbContext context, IPasswordHasher<User> passwordHasher)
{
    public async Task InitialiseAsync()
    {
        try
        {
            if (context.Database.HasPendingModelChanges())
            {
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            if (!await context.Set<User>().AnyAsync())
            {
                var adminUser = new User
                {
                    FirstName = SystemRoles.SuperAdmin,
                    LastName = SystemRoles.SuperAdmin,
                    UserName = SystemRoles.SuperAdmin,
                    NormalizedUserName = SystemRoles.SuperAdmin.ToUpper(),
                    Department = SystemRoles.SuperAdmin,
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    IsDeleted = false,
                    SecurityStamp = "random",
                    ConcurrencyStamp = "random",
                };
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Password1234!");
                context.Set<User>().Add(adminUser);

                await context.SaveChangesAsync();
            }

            if (!await context.Set<SystemRole>().AnyAsync())
            {
                context.Set<SystemRole>().Add(new SystemRole
                {
                    Name = SystemRoles.SuperAdmin,
                    NormalizedName = SystemRoles.SuperAdmin.ToUpper(),
                    IsDeleted = false,
                });

                context.Set<SystemRole>().Add(new SystemRole
                {
                    Name = SystemRoles.Regular,
                    NormalizedName = SystemRoles.Regular.ToUpper(),
                    IsDeleted = false,
                });

                await context.SaveChangesAsync();
            }

            if (!await context.Set<SystemRoleAssignment>().AnyAsync())
            {
                var adminRole = await context.Set<SystemRole>()
                    .FirstOrDefaultAsync(r => r.Name == SystemRoles.SuperAdmin);

                var adminUser = await context.Set<User>()
                    .FirstOrDefaultAsync(u => u.NormalizedEmail == "ADMIN@GMAIL.COM");

                if (adminRole != null && adminUser != null)
                {
                    context.Set<SystemRoleAssignment>().Add(new SystemRoleAssignment
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id,
                    });
                }

                await context.SaveChangesAsync();
            }

            if (!await context.ClientRoles.AnyAsync())
            {
                context.ClientRoles.Add(new ClientRole
                {
                    Name = ClientRoles.Owner,
                    IsDeleted = false,
                });
                context.ClientRoles.Add(new ClientRole
                {
                    Name = ClientRoles.ClientAdmin,
                    IsDeleted = false,
                });
                context.ClientRoles.Add(new ClientRole
                {
                    Name = ClientRoles.Operator,
                    IsDeleted = false,
                });
                await context.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding data: {ex.Message}");
            throw;
        }
    }
}