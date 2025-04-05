using System.Reflection;
using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Features;
using Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
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

            config.AddOpenBehavior(typeof(LoggingPipelineBehavior<,>));
        });

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

        return services;
    }
}
