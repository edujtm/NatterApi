using System.Data;

namespace Natter.Shared.Architecture;

public interface IConnectionFactory
{
    IDbConnection GetConnection();
}