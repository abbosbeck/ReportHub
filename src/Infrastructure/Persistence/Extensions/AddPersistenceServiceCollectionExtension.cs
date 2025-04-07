using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Infrastructure.Persistence.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure.Persistence.Extensions;
public static class AddPersistenceServiceCollectionExtension
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IOptions<KeyVaultOptions> keyVaultOptions)
    {
        var connectionString = GetConnectionString(keyVaultOptions);
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, SoftDeletableInterceptor>();
        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<AppDbContextInitializer>();
        return services;
    }

    private static string GetConnectionString(IOptions<KeyVaultOptions> keyVaultOptions)
    {
        var keyVaultUrl = keyVaultOptions.Value.KeyVaultURL;
        var keyVaultClientId = keyVaultOptions.Value.ClientId;
        var keyVaultClientSecret = keyVaultOptions.Value.ClientSecret;
        var keyVaultDirectoryId = keyVaultOptions.Value.DirectoryID;

        var credential = new ClientSecretCredential(
            keyVaultDirectoryId,
            keyVaultClientId,
            keyVaultClientSecret);
        var client = new SecretClient(new Uri(keyVaultUrl), credential);

        return client.GetSecret("DatabaseConnectionString").Value.Value;
    }
}
