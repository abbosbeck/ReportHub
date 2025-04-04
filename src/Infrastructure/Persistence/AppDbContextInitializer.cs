using Domain.Entity;
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

public class AppDbContextInitializer(AppDbContext context)
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
                context.Set<User>().Add(new User
                {
                    FirstName = "Alex",
                    LastName = "Turner",
                });
                context.Set<User>().Add(new User
                {
                    FirstName = "Ivan",
                    LastName = "Ivanov",
                });
                context.Set<User>().Add(new User
                {
                    FirstName = "Gulom",
                    LastName = "Akilov",
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
