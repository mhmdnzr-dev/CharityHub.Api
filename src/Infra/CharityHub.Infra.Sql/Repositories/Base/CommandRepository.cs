namespace CharityHub.Infra.Sql.Repositories.Base;

using CharityHub.Core.Domain.ValueObjects;
using CharityHub.Core.Presistance.Interfaces.Base;
using CharityHub.Infra.Sql.Data.DbContexts;

public class CommandRepository<T>(CharityHubCommandDbContext commandDbContext) : ICommandRepository<T> where T : BaseEntity
{
    protected readonly CharityHubCommandDbContext _commandDbContext = commandDbContext;

    #region Sync Methods

    public void Add(T entity)
    {
        _commandDbContext.Set<T>().Add(entity);
        _commandDbContext.SaveChanges();
    }

    public void Update(T entity)
    {
        _commandDbContext.Set<T>().Update(entity);
        _commandDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = _commandDbContext.Set<T>().Find(id);
        if (entity != null)
        {
            _commandDbContext.Set<T>().Remove(entity);
            _commandDbContext.SaveChanges();
        }
    }

    #endregion

    #region Async Methods

    public async Task AddAsync(T entity)
    {
        await _commandDbContext.Set<T>().AddAsync(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _commandDbContext.Set<T>().Update(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _commandDbContext.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _commandDbContext.Set<T>().Remove(entity);
            await _commandDbContext.SaveChangesAsync();
        }
    }

    #endregion
}
