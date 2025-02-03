namespace CharityHub.Core.Contract;

using System.Reflection;

using Configuration.Models;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Primitives.Handlers;
using Primitives.Repositories;




public static class DependencyInjection
{
    public static void AddContract(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Register MediatR (scanning for handlers)
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

        // Ensure all other dependencies and services
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<SmsProviderOptions>(configuration.GetSection("SmsProvider"));
        services.Configure<FileOptions>(configuration.GetSection("FileSettings"));
    }
}