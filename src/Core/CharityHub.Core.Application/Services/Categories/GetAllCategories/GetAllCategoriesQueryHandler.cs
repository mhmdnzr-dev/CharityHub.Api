namespace CharityHub.Core.Application.Services.Categories.GetAllCategories;

using Contract.Categories.Queries;
using Contract.Categories.Queries.GetAllCategories;

using Contract.Primitives.Handlers;


public class GetAllCategoriesQueryHandler: IQueryHandler<GetAllCategoriesQuery, List<AllCategoriesResponseDto>>
{
    private readonly ICategoryQueryRepository _categoryQueryRepository;

    public GetAllCategoriesQueryHandler(ICategoryQueryRepository categoryQueryRepository)
    {
        _categoryQueryRepository = categoryQueryRepository;
    }

    public async Task<List<AllCategoriesResponseDto>> Handle(GetAllCategoriesQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _categoryQueryRepository.GetAllAsync(query);
        return result;
    }
}