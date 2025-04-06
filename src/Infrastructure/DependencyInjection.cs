using Application.Common.Interfaces;
using Application.Common.JWT;
using Domain.Entities;
using Infrastructure.Authentication.Extensions;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
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
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();

        return services;
    }
}
