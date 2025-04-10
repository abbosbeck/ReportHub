using Application.Common.Interfaces;
using Application.Common.Services;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KeyVaultOptions>(
            configuration.GetSection(nameof(KeyVaultOptions)));
        using var serviceProvider = services.BuildServiceProvider();
        var keyVaultOptions = serviceProvider.GetRequiredService<IOptions<KeyVaultOptions>>();
        var jwtOptions = serviceProvider.GetRequiredService<IOptions<JwtOptions>>();

        services.AddJwtAuthentication(jwtOptions);
        services.AddAuthorization();
        services.AddPersistence(keyVaultOptions);
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordHasher<Client>, PasswordHasher<Client>>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();

        services.AddIdentity(configuration);

        return services;
    }

    private static void AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = true;

            options.User.RequireUniqueEmail = true;
        })
            .AddRoles<SystemRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IEmailSender, EmailService>();

        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
    }
}