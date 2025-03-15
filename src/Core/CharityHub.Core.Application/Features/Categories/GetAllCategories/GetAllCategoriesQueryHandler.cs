namespace CharityHub.Core.Application.Features.Categories.GetAllCategories;

using Contract.Features.Categories.Queries;
using Contract.Features.Categories.Queries.GetAllCategories;

using Infra.Identity.Interfaces;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCategoriesQueryHandler : QueryHandlerBase<GetAllCategoriesQuery, List<AllCategoriesResponseDto>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;


    public GetAllCategoriesQueryHandler(IMemoryCache cache, ITokenService tokenService,
        IHttpContextAccessor httpContextAccessor, ICategoryQueryRepository categoryQueryRepository) : base(cache,
        tokenService, httpContextAccessor)
    {
        _categoryQueryRepository = categoryQueryRepository;
    }

    public override async Task<List<AllCategoriesResponseDto>> Handle(GetAllCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _categoryQueryRepository.GetAllAsync(query);
        return result;
    }
}