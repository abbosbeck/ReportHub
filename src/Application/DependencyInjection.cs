﻿using System.Reflection;
using Application.Common.Behaviors;
using Application.Common.Configurations;
using Application.Common.Interfaces.Authorization;
using Application.Common.Interfaces.Services;
using Application.Common.Services;
using Application.Reports.ExportReportsToFile;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);

        services.Configure<JwtOptions>(
            configuration.GetSection(nameof(JwtOptions)));

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);

            config.AddOpenBehavior(typeof(AuthorizationPipelineBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IReportGeneratorAsFileService, ReportGeneratorAsFileService>();
        services.AddScoped<IImportDataFromFileService, ImportDataFromFileServic>();
        services.AddScoped<IExportReportsToFileQueryHandler, ExportReportsToFileQueryHandler>();
        services.AddHttpContextAccessor();

        services.AddScoped<IClientIdProvider, ClientIdProvider>();

        return services;
    }
}
