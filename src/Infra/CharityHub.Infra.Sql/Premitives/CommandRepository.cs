using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Core.Domain.ValueObjects;
using CharityHub.Infra.Sql.Data.DbContexts;
namespace CharityHub.Infra.Sql.Premitives;



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
        if (entity is not null)
        {
            entity.IsActive = false;
            _commandDbContext.Set<T>().Update(entity);
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
            entity.IsActive = false;
            _commandDbContext.Set<T>().Update(entity);
            await _commandDbContext.SaveChangesAsync();
        }
    }

    #endregion
}
