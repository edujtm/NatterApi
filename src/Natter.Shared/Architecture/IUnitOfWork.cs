

namespace Natter.Shared.Architecture;


public interface IUnitOfWork : IDisposable
{
    Task RollbackAsync();
    Task CommitAsync();
}