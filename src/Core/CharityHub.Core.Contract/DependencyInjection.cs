namespace CharityHub.Core.Contract;

using Charity.Commands.CreateCharity;

using CharityHub.Core.Contract.Primitives.Handlers;

using Configuration.Models;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
    public static void AddContract(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();

        
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IQueryHandler<,>), typeof(ICommandHandler<>))  // Scan for handlers
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>))) // Query handlers
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>))) // Command handlers
            .AsImplementedInterfaces()
            .WithScopedLifetime());

// MediatR Handler Registrations
        services.AddTransient(typeof(IRequestHandler<,>), typeof(MediatorQueryHandlerAdapter<,>));
        services.AddTransient(typeof(IRequestHandler<CreateCharityCommand, int>), typeof(MediatorCommandHandlerAdapter<CreateCharityCommand>));




        
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<SmsProviderOptions>(configuration.GetSection("SmsProvider"));
        services.Configure<FileOptions>(configuration.GetSection("FileSettings"));
    }
}
