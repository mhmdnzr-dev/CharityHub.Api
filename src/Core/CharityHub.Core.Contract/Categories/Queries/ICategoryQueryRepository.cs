namespace CharityHub.Core.Contract.Categories.Queries;

using Domain.Entities;
using Domain.ValueObjects;

using GetAllCategories;

using Primitives.Repositories;

public interface ICategoryQueryRepository : IQueryRepository<Category>
{
    Task<List<AllCategoriesResponseDto>> GetAllAsync(GetAllCategoriesQuery query);
}