namespace Natter.Infrastructure.DbAccess;
using System.Data.Common;

using Natter.Shared.Architecture;


public class UnitOfWork : IUnitOfWork
{
    private readonly DbTransaction transaction;
    public DbConnection Connection { get; }

    public bool IsDisposed { get; private set; }

    public UnitOfWork(DbConnection connection)
    {
        Connection = connection;
        transaction = Connection.BeginTransaction();
    }

    public async Task RollbackAsync() => await transaction.RollbackAsync();

    public async Task CommitAsync() => await transaction.CommitAsync();

    public void Dispose()
    {
        transaction?.Dispose();

        IsDisposed = true;
    }
}
