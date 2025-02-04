namespace CharityHub.Core.Contract.Primitives.Repositories;

using CharityHub.Core.Domain.ValueObjects;

public interface IQueryRepository<T> where T : BaseEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
