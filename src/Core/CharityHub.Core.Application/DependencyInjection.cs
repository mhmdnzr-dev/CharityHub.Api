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

using Contract.Campaign.Queries;
using Contract.Campaign.Queries.GetAllCampaigns;
using Contract.Campaign.Queries.GetCampaignById;
using Contract.Charity.Queries;
using Contract.Charity.Queries.GetAllCharities;
using Contract.Charity.Queries.GetCharityById;
using Contract.Primitives.Models;

using Infra.Sql.Repositories.Campaign;
using Infra.Sql.Repositories.Charities;

using Services.Campaign.Queries.GetAllCampaigns;
using Services.Campaign.Queries.GetCampaignById;
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


        
       
        #region Term Query Repositores DI
        services.AddScoped<ITermQueryRepository, TermQueryRepository>();
        services.AddScoped<IQueryHandler<GetLastTermQuery, LastTermResponseDto>, GetLastTermQueryHandler>();
        #endregion
        
        #region Charity Query Repositories DI
        services.AddScoped<ICharityQueryRepository, CharityQueryRepository>();
        services.AddScoped<IQueryHandler<GetAllCharitiesQuery, IEnumerable<AllCharitiesResponseDto>>, GetAllCharitiesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCharityByIdQuery, CharityByIdResponseDto>, GetCharityByIdQueryHandler>();
        #endregion

        #region Campaign Query Repostories DI
        services.AddScoped<ICampaignQueryRepository, CampaignQueryRepository>();
        services.AddScoped<IQueryHandler<GetAllCampaignQuery, PagedData<AllCampaignResponseDto>>, GetAllCampaignsQueryHandler>();
        services.AddScoped<IQueryHandler<GetCampaignByIdQuery, CampaignByIdResponseDto>, GetCampaignByIdQueryHandler>();

        #endregion
        
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
