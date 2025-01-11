namespace CharityHub.Core.Presistance.Interfaces.Base;

using CharityHub.Core.Domain.ValueObjects;

public interface IQueryRepository<T> where T : BaseEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
