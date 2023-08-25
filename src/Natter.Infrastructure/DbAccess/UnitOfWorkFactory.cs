namespace Natter.Infrastructure.DbAccess;
using System.Data.Common;
using Natter.Shared.Architecture;


public class UnitOfWorkFactory : IUnitOfWorkFactory, IConnectionFactory, IDisposable
{
    private readonly DbConnection _connection;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkFactory(DbConnection connection) => _connection = connection;

    public DbConnection GetConnection()
    {
        if (!IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "There is not current unit of work from which to get a connection. Call BeginTransaction first"
            );
        }
        return _unitOfWork!.Connection;
    }

    public IUnitOfWork Create()
    {
        if (IsUnitOfWorkOpen)
        {
            throw new InvalidOperationException(
                "Cannot begin a transaction before the unit of work from the last one is disposed"
            );
        }
        _unitOfWork = new UnitOfWork(_connection);
        return _unitOfWork;
    }

    public void Dispose() => throw new NotImplementedException();
}
