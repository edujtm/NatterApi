namespace Natter.Infrastructure.DbAccess;
using System.Data.Common;
using Natter.Shared.Architecture;


public class UnitOfWorkFactory : IUnitOfWorkFactory, IConnectionFactory, IDisposable
{
    private readonly DbConnection _connection;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(this._unitOfWork == null || this._unitOfWork.IsDisposed);

    public UnitOfWorkFactory(DbConnection connection) => this._connection = connection;

    public DbConnection GetConnection()
    {
        if (!this.IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "There is not current unit of work from which to get a connection. Call BeginTransaction first"
            );
        }
        return this._unitOfWork!.Connection;
    }

    public IUnitOfWork Create()
    {
        if (this.IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "Cannot begin a transaction before the unit of work from the last one is disposed"
            );
        }
        this._unitOfWork = new UnitOfWork(this._connection);
        return this._unitOfWork;
    }

    public void Dispose() => throw new NotImplementedException();
}
