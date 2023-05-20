using System.Data.Common;

using Natter.Shared.Architecture;

namespace Natter.Infrastructure.DbAccess;


public class UnitOfWork : IUnitOfWork
{
    private readonly DbTransaction _transaction;
    public DbConnection Connection { get; }

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(DbConnection connection)
    {
        Connection = connection;
        _transaction = Connection.BeginTransaction();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();

        IsDisposed = true;
    }
}