namespace CharityHub.Infra.Sql.Primitives;

using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.ValueObjects;
using CharityHub.Infra.Sql.Data.DbContexts;

using Exceptions;

using Microsoft.EntityFrameworkCore;


public class QueryRepository<T>(CharityHubQueryDbContext queryDbContext) : IQueryRepository<T> where T : BaseEntity
{
    protected readonly CharityHubQueryDbContext _queryDbContext = queryDbContext;

    #region Sync Methods

    public T GetById(int id)
    {
        var entity = _queryDbContext.Set<T>().FirstOrDefault(data => data.Id == id);
        if (entity == null)
        {
            throw InfrastructureException.DatabaseError($"{typeof(T).Name} with ID {id} was not found.");
        }
        return entity;
    }

    public IEnumerable<T> GetAll()
    {
        var entities = _queryDbContext.Set<T>().ToList();
        if (!entities.Any())
        {
            throw InfrastructureException.DatabaseError($"No {typeof(T).Name} records found.");
        }
        return entities;
    }

    #endregion

    #region Async Methods

    public async Task<T> GetByIdAsync(int id)
    {
        var entity = await _queryDbContext.Set<T>().FirstOrDefaultAsync(data => data.Id == id);
        if (entity == null)
        {
            throw InfrastructureException.DatabaseError($"{typeof(T).Name} with ID {id} was not found.");
        }
        return entity;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var entities = await _queryDbContext.Set<T>().ToListAsync();
        if (!entities.Any())
        {
            throw InfrastructureException.DatabaseError($"No {typeof(T).Name} records found.");
        }
        return entities;
    }

    #endregion
}

