using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Primitives.Validations;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Contract;

public static class DependencyInjection
{
    public static IServiceCollection AddContract(this IServiceCollection services)
    {
        // Register command and query handlers automatically using assembly scanning
        services.Scan(scan => scan
            .FromAssembliesOf(typeof(ICommandHandler<>), typeof(IQueryHandler<,>))
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        // Register Mediator Adapters
        services.AddScoped(typeof(IRequestHandler<>), typeof(MediatorCommandHandlerAdapter<>));
        services.AddScoped(typeof(IRequestHandler<,>), typeof(MediatorQueryHandlerAdapter<,>));

        // Add MediatR pipeline behavior for FluentValidation
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationBehavior<,>));


        return services;
    }
}

