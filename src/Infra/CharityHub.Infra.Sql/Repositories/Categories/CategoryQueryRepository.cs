namespace CharityHub.Infra.Sql.Repositories.Categories;

using Core.Contract.Categories.Queries;
using Core.Contract.Categories.Queries.GetAllCategories;
using Core.Contract.Primitives.Repositories;
using Core.Domain.Entities;

using Data.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Primitives;

public class CategoryQueryRepository(CharityHubQueryDbContext queryDbContext, ILogger<CategoryQueryRepository> logger)
    : QueryRepository<Category>(queryDbContext), ICategoryQueryRepository
{
    public async Task<List<AllCategoriesResponseDto>> GetAllAsync(GetAllCategoriesQuery dto)
    {
        var query = _queryDbContext.Categories.AsQueryable();
        var dtoModel = query.Select(data => new AllCategoriesResponseDto { Id = data.Id, Name = data.Name, });
        var result = await dtoModel.ToListAsync();
        return result;
    }
}