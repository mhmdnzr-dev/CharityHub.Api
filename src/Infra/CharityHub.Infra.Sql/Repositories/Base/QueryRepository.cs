using CharityHub.Core.Presistance.Interfaces.Base;
using CharityHub.Infra.Sql.Data.DbContexts;

using Microsoft.EntityFrameworkCore;

namespace CharityHub.Infra.Sql.Repositories.Base;


public class QueryRepository<T>(CharityHubQueryDbContext queryDbContext) : IQueryRepository<T> where T : class
{
    protected readonly CharityHubQueryDbContext _queryDbContext = queryDbContext;

    #region Sync Methods

    public T GetById(int id)
    {
        return _queryDbContext.Set<T>().Find(id);
    }

    public IEnumerable<T> GetAll()
    {
        return _queryDbContext.Set<T>().ToArray();
    }

    #endregion

    #region Async Methods

    public async Task<T> GetByIdAsync(int id)
    {
        return await _queryDbContext.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _queryDbContext.Set<T>().ToArrayAsync();
    }

    #endregion
}

