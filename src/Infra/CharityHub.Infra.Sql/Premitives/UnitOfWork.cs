using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;

using Microsoft.EntityFrameworkCore.Storage;

namespace CharityHub.Infra.Sql.Premitives;

public class UnitOfWork : IUnitOfWork
{
    private readonly CharityHubCommandDbContext _commandDbContext;

    private IDbContextTransaction _transaction;
    private bool _disposed = false;

    public UnitOfWork(CharityHubCommandDbContext commandDbContext)
    {
        _commandDbContext = commandDbContext;
    }

    #region Sync Methods

    public void BeginTransaction()
    {
        _transaction = _commandDbContext.Database.BeginTransaction();
    }

    public void Commit()
    {
        try
        {
            _commandDbContext.SaveChanges();
            _transaction.Commit();
        }
        catch
        {
            _transaction.Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        _transaction?.Rollback();
    }

    #endregion

    #region Async Methods

    public async Task BeginTransactionAsync()
    {
        _transaction = await _commandDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await _commandDbContext.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await _transaction.RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    #endregion

    #region IDisposable Implementation

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _commandDbContext?.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
