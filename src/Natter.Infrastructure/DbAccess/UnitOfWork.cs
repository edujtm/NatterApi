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
        this.Connection = connection;
        this.transaction = this.Connection.BeginTransaction();
    }

    public async Task RollbackAsync() => await this.transaction.RollbackAsync();

    public async Task CommitAsync() => await this.transaction.CommitAsync();

    public void Dispose()
    {
        this.transaction?.Dispose();

        this.IsDisposed = true;
    }
}
