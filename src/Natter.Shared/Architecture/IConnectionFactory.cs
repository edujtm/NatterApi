namespace Natter.Shared.Architecture;
using System.Data.Common;

public interface IConnectionFactory
{
    DbConnection GetConnection();
}
