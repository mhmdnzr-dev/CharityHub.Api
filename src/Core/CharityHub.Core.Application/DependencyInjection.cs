using CharityHub.Core.Contract.Primitives.Handlers;


using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace CharityHub.Core.Application;

using System.Reflection;
using Contract.Primitives.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;


public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        /*#region Category Query Repositores DI

        services.AddScoped<ICategoryQueryRepository, CategoryQueryRepository>();
        services
            .AddScoped<IQueryHandler<GetAllCategoriesQuery, List<AllCategoriesResponseDto>>,
                GetAllCategoriesQueryHandler>();

        #endregion


        #region Term Query Repositores DI

        services.AddScoped<ITermQueryRepository, TermQueryRepository>();
        services.AddScoped<IQueryHandler<GetLastTermQuery, LastTermResponseDto>, GetLastTermQueryHandler>();

        #endregion

        #region Charity Query Repositories DI

        services.AddScoped<ICharityQueryRepository, CharityQueryRepository>();
        services
            .AddScoped<IQueryHandler<GetAllCharitiesQuery, PagedData<AllCharitiesResponseDto>>,
                GetAllCharitiesQueryHandler>();
        services.AddScoped<IQueryHandler<GetCharityByIdQuery, CharityByIdResponseDto>, GetCharityByIdQueryHandler>();

        #endregion

        #region Charity Command Repositories DI
        services.AddScoped<ICharityCommandRepository, CharityCommandRepository>();
        services.AddScoped<ICommandHandler<CreateCharityCommand>, CreateCharityCommandHandler>();
        #endregion

        #region Campaign Query Repostories DI

        services.AddScoped<ICampaignQueryRepository, CampaignQueryRepository>();
        services
            .AddScoped<IQueryHandler<GetAllCampaignQuery, PagedData<AllCampaignResponseDto>>,
                GetAllCampaignsQueryHandler>();
        services.AddScoped<IQueryHandler<GetCampaignByIdQuery, CampaignByIdResponseDto>, GetCampaignByIdQueryHandler>();
        services
            .AddScoped<IQueryHandler<GetCampaignsByCharityIdQuery, PagedData<CampaignsByCharityIdResponseDto>>,
                GetCampaignsByCharityIdQueryHandler>();

        #endregion
        */

        // Automatically register all AbstractValidator<T> implementations in the assembly (for FluentValidation)
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
    }
}