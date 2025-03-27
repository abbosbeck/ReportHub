using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Persistence.Repositories;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = GetConnectionString(configuration);

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeletableInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<AppDbContextInitializer>();

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        var keyVaultURL = configuration.GetSection("KeyVault:KeyVaultURL");
        var keyVaultClientId = configuration.GetSection("KeyVault:ClientId");
        var keyVaultClientSecret = configuration.GetSection("KeyVault:ClientSecret");
        var keyVaultDirectoryID = configuration.GetSection("KeyVault:DirectoryID");
        
        var credential = new ClientSecretCredential(
                    keyVaultDirectoryID.Value!.ToString(),
                    keyVaultClientId.Value!.ToString(),
                    keyVaultClientSecret.Value!.ToString());
        var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), credential);
        
        return client.GetSecret("DatabaseConnectionString").Value.Value.ToString();
    }
}