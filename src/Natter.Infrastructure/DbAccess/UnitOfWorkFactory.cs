using System.Data;
using Npgsql;
using Natter.Shared.Architecture;

namespace Natter.Infrastructure.DbAccess;


public class UnitOfWorkFactory : IUnitOfWorkFactory, IConnectionFactory
{
    private readonly NpgsqlConnection _connection;
    private UnitOfWork? _unitOfWork;

    private bool IsUnitOfWorkOpen => !(_unitOfWork == null || _unitOfWork.IsDisposed);

    public UnitOfWorkFactory(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public IDbConnection GetConnection()
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
}