namespace CharityHub.Infra.Sql.Primitives;

using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;

using Core.Domain.Entities.Common;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

public class CommandRepository<T>(CharityHubCommandDbContext commandDbContext) : ICommandRepository<T> where T : BaseEntity
{
    protected readonly CharityHubCommandDbContext _commandDbContext = commandDbContext;

    #region Sync Methods
    public void InsertRange(IEnumerable<T> entities)
    {
        _commandDbContext.BulkInsert(entities);
    }

    
    public void Insert(T entity)
    {
        _commandDbContext.Set<T>().Attach(entity);
        _commandDbContext.SaveChanges();
    }

    public void Update(T entity)
    {
        _commandDbContext.Set<T>()
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdate(setters => setters
                .SetProperty(e => e, entity)); // Updating all properties
    }

    public void Delete(int id)
    {
        _commandDbContext.Set<T>()
            .Where(e => e.Id == id)
            .ExecuteUpdate(setters => setters.SetProperty(e => e.IsActive, false));
    }


    #endregion

    #region Async Methods
    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _commandDbContext.BulkInsertAsync(entities);
    }

    
    public async Task InsertAsync(T entity)
    {
        await _commandDbContext.Set<T>().AddAsync(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        await _commandDbContext.Set<T>()
            .Where(e => e.Id == entity.Id)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(e => e, entity)); // Updating all properties
    }

    public async Task DeleteAsync(int id)
    {
        await _commandDbContext.Set<T>()
            .Where(e => e.Id == id)
            .ExecuteUpdateAsync(setters => setters.SetProperty(e => e.IsActive, false));
    }


    #endregion
}
