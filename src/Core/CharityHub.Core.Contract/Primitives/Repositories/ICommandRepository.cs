namespace CharityHub.Core.Contract.Primitives.Repositories;

using Domain.Entities.Common;

public interface ICommandRepository<T> where T : BaseEntity
{
    void InsertRange(IEnumerable<T> entities);
    void Insert(T entity);
    void Update(T entity);
    void Delete(int id);


    Task InsertRangeAsync(IEnumerable<T> entities);
    Task InsertAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
