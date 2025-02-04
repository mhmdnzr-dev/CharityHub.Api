namespace CharityHub.Core.Application.Services.Categories.GetAllCategories;

using Contract.Categories.Queries;
using Contract.Categories.Queries.GetAllCategories;

using Contract.Primitives.Handlers;

using Infra.Identity.Interfaces;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCategoriesQueryHandler: QueryHandlerBase<GetAllCategoriesQuery, List<AllCategoriesResponseDto>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;


    public GetAllCategoriesQueryHandler(IMemoryCache cache, ITokenService tokenService, ICategoryQueryRepository categoryQueryRepository) : base(cache, tokenService)
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