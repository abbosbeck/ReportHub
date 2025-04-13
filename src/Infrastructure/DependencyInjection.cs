using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Repositories;
using Application.Common.Services;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Extensions;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
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
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISystemRoleAssignmentRepository, SystemRoleAssignmentRepository>();
        services.AddScoped<ISystemRoleRepository, SystemRoleRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientRoleAssignmentRepository, ClientRoleAssignmentRepository>();
        services.AddScoped<IClientRoleRepository, ClientRoleRepository>();

        services.AddIdentity(configuration);

        services.AddDataProtection()
            .PersistKeysToDbContext<AppDbContext>()
            .SetApplicationName("ReportHub");

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
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IEmailService, EmailService>();

        services.Configure<EmailOptions>(configuration.GetSection(nameof(EmailOptions)));
        services.Configure<SmtpOptions>(configuration.GetSection(nameof(SmtpOptions)));
    }
}