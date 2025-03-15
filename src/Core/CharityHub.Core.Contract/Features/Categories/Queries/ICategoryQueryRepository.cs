namespace CharityHub.Core.Contract.Features.Categories.Queries;

using Domain.Entities;

using GetAllCategories;

using Primitives.Repositories;

public interface ICategoryQueryRepository : IQueryRepository<Category>
{
    Task<List<AllCategoriesResponseDto>> GetAllAsync(GetAllCategoriesQuery query);
}