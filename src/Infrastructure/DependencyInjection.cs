﻿using Application.Common.Configurations;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.External.Countries;
using Application.Common.Interfaces.External.CurrencyExchange;
using Application.Common.Interfaces.Repositories;
using Application.Common.Interfaces.Time;
using Domain.Entities;
using Infrastructure.Authentication;
using Infrastructure.Authentication.Extensions;
using Infrastructure.External;
using Infrastructure.Jobs;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Extensions;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Time;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;

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
        services.AddSingleton<AppMongoDbContext>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISystemRoleAssignmentRepository, SystemRoleAssignmentRepository>();
        services.AddScoped<ISystemRoleRepository, SystemRoleRepository>();
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IClientRoleAssignmentRepository, ClientRoleAssignmentRepository>();
        services.AddScoped<IClientRoleRepository, ClientRoleRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IItemRepository, ItemRepository>();
        services.AddScoped<IPlanRepository, PlanRepository>();
        services.AddScoped<IPlanItemRepository, PlanItemRepository>();
        services.AddScoped<ILogRepository, LogRepository>();
        services.AddScoped<IReportScheduleRepository, ReportScheduleRepository>();
        services.AddSingleton<IReportScheduleService, ReportScheduleService>();
        services.AddSingleton<IDateTimeService, DateTimeService>();
        services.AddHttpClient<ICountryService, CountryService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["CountriesApi"] ??
                                             throw new InvalidOperationException("Countries api url is null"));
        });

        services.AddHttpClient<ICurrencyExchangeService, CurrencyExchangeService>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(configuration["ExchangeRate"] ??
                                             throw new InvalidOperationException("Exchange rate api url is null"));
        });

        services.AddIdentity(configuration);

        services.AddDataProtection()
            .PersistKeysToDbContext<AppDbContext>()
            .SetApplicationName("ReportHub");

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

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