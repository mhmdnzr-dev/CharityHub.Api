﻿namespace CharityHub.Core.Contract;


using Configuration.Models;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Primitives.Repositories;
using Primitives.Validations;

public static class DependencyInjection
{
    public static void AddContract(this IServiceCollection services, IConfiguration configuration)
    {
        // Bind configuration options properly
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<OpenIdOptions>(configuration.GetSection("OpenId"));
        services.Configure<SmsProviderOptions>(configuration.GetSection("SmsProvider"));
        services.Configure<FileOptions>(configuration.GetSection("FileSettings"));

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Register MediatR handlers correctly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        // Register all repositories
        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        services.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryRepository<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );

        // Add MediatR pipeline behavior for FluentValidation
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));
    }

}