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
            await context.Database.MigrateAsync();
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
                    FirstName = "Admin",
                    LastName = "Admin",
                    Department = "Admin",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "ADMIN@GMAIL.COM",
                    EmailConfirmed = true,
                    IsDeleted = false,
                };
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Password1234!");
                context.Set<User>().Add(adminUser);

                var regularUser = new User
                {
                    FirstName = "John",
                    LastName = "John",
                    Department = "Workers",
                    Email = "user@gmail.com",
                    NormalizedEmail = "USER@GMAIL.COM",
                    EmailConfirmed = true,
                    IsDeleted = false,
                };
                regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "Password1234!");
                context.Set<User>().Add(regularUser);

                await context.SaveChangesAsync();
            }

            if (!await context.Set<SystemRole>().AnyAsync())
            {
                context.Set<SystemRole>().Add(new SystemRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    IsDeleted = false,
                });
                context.Set<SystemRole>().Add(new SystemRole
                {
                    Name = "User",
                    NormalizedName = "USER",
                    IsDeleted = false,
                });
                await context.SaveChangesAsync();
            }

            if (!await context.Set<UserSystemRole>().AnyAsync())
            {
                var adminRole = await context.Set<SystemRole>().FirstOrDefaultAsync(r => r.Name == "Admin");
                var adminUser = await context.Set<User>().FirstOrDefaultAsync(u => u.FirstName == "Admin");

                if (adminRole != null && adminUser != null)
                {
                    context.Set<UserSystemRole>().Add(new UserSystemRole
                    {
                        UserId = adminUser.Id,
                        RoleId = adminRole.Id,
                    });
                }

                var userRole = await context.Set<SystemRole>().FirstOrDefaultAsync(r => r.Name == "User");
                var justUser = await context.Set<User>().FirstOrDefaultAsync(u => u.FirstName == "John");
                if (userRole != null && justUser != null)
                {
                    context.Set<UserSystemRole>().Add(new UserSystemRole
                    {
                        UserId = justUser.Id,
                        RoleId = userRole.Id,
                    });
                }

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