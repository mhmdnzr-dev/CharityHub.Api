using CharityHub.Core.Contract.Primitives.Handlers;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Contract;
public static class DependencyInjection
{
    public static IServiceCollection AddContract(this IServiceCollection services)
    {
        // Register all query and command handlers automatically
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(IQueryHandler<,>), typeof(ICommandHandler<>))  // Scan for handlers of both commands and queries
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))  // Register query handlers
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))  // Register command handlers
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register Mediator Adapters
        services.AddScoped(typeof(IRequestHandler<>), typeof(MediatorCommandHandlerAdapter<>));  // For commands
        services.AddScoped(typeof(IRequestHandler<,>), typeof(MediatorQueryHandlerAdapter<,>));  // For queries



        return services;
    }
}
