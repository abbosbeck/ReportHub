using Application.Common.Interfaces;
using Application.Features;
using Infrastructure.Authentication.Extensions;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<KeyVaultOptions>(
            configuration.GetSection(nameof(KeyVaultOptions)));
        using var serviceProvider = services.BuildServiceProvider();
        var keyVaultOptions = serviceProvider.GetRequiredService<IOptions<KeyVaultOptions>>();
        var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>();

        services.AddJwtAuthentication(jwtOptions);
        services.AddPersistence(keyVaultOptions);
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
