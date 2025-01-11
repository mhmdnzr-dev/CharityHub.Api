namespace CharityHub.Core.Presistance.Interfaces.Base;

public interface IQueryRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();

    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
}
