using CharityHub.Core.Domain.ValueObjects;

namespace CharityHub.Core.Contract.Primitives.Repositories;

public interface ICommandRepository<T> where T : BaseEntity
{
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);

    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
