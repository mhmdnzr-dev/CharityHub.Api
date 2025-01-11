using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.ValueObjects;
using CharityHub.Infra.Sql.Data.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Premitives;


public class QueryRepository<T>(CharityHubQueryDbContext queryDbContext) : IQueryRepository<T> where T : BaseEntity
{
    protected readonly CharityHubQueryDbContext _queryDbContext = queryDbContext;

    #region Sync Methods

    public T GetById(int id)
    {
        return _queryDbContext.Set<T>().First(data => data.Id == id);
    }

    public IEnumerable<T> GetAll()
    {
        return [.. _queryDbContext.Set<T>()];
    }

    #endregion

    #region Async Methods

    public async Task<T> GetByIdAsync(int id)
    {
        return await _queryDbContext.Set<T>().FirstAsync(data => data.Id == id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _queryDbContext.Set<T>().ToArrayAsync();
    }

    #endregion
}

