using Npgsql;

using Natter.Shared.Architecture;

namespace Natter.Infrastructure.DbAccess;


public class UnitOfWork : IUnitOfWork
{
    private readonly NpgsqlTransaction _transaction;
    public NpgsqlConnection Connection { get; }

    public bool IsDisposed { get; private set; } = false;

    public UnitOfWork(NpgsqlConnection connection)
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