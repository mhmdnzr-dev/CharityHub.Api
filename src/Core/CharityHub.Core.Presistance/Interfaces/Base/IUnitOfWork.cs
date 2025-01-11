namespace CharityHub.Core.Presistance.Interfaces.Base;


public interface IUnitOfWork : IDisposable
{
    #region Sync Methods
    void BeginTransaction();
    void Commit();
    void Rollback();
    #endregion

    #region Async Methods
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
    #endregion
}
