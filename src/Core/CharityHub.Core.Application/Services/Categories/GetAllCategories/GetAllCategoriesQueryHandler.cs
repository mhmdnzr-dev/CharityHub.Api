namespace CharityHub.Core.Application.Services.Categories.GetAllCategories;

using Contract.Categories.Queries;
using Contract.Categories.Queries.GetAllCategories;

using Contract.Primitives.Handlers;

using Microsoft.Extensions.Caching.Memory;

using Primitives;

public class GetAllCategoriesQueryHandler: QueryHandlerBase<GetAllCategoriesQuery, List<AllCategoriesResponseDto>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;


    public GetAllCategoriesQueryHandler(IMemoryCache cache, ICategoryQueryRepository categoryQueryRepository) : base(cache)
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