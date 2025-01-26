using CharityHub.Core.Application.Configuration.Models;
using CharityHub.Core.Application.Services.Terms.Queries.GetLastTerm;
using CharityHub.Core.Contract.Primitives.Handlers;
using CharityHub.Core.Contract.Terms.Queries;
using CharityHub.Core.Contract.Terms.Queries.GetLastTerm;
using CharityHub.Infra.Sql.Repositories.Terms;

using FluentValidation;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Application;

using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Charity.Queries.GetCharityById;

using Infra.Sql.Repositories.Charities;

using Services.Charities.Queries.GetAllCharities;
using Services.Charities.Queries.GetCharityById;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        // Bind configuration models to use with IOptions<T>    
        services.Configure<LoggingOptions>(configuration.GetSection("Logging"));
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.Configure<SmsProviderOptions>(configuration.GetSection("SmsProvider"));


        services.AddScoped<ITermQueryRepository, TermQueryRepository>();
        services.AddScoped<ICharityQueryRepository, CharityQueryRepository>();

        // Register Mediator Adapters
        services.AddScoped<IQueryHandler<GetLastTermQuery, LastTermResponseDto>, GetLastTermQueryHandler>();
        services.AddScoped<IQueryHandler<GetAllCharitiesQuery, IEnumerable<AllCharitiesResponseDto>>, GetAllCharitiesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCharityByIdQuery, CharityByIdResponseDto>, GetCharityByIdQueryHandler>();


        // Automatically register all AbstractValidator<T> implementations in the assembly (for FluentValidation)
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Register MediatR and ensure it's using the current assembly
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}
