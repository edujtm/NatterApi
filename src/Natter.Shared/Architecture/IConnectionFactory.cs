using System.Data.Common;

namespace Natter.Shared.Architecture;

public interface IConnectionFactory
{
    DbConnection GetConnection();
}