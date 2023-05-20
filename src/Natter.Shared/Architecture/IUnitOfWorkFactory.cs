

namespace Natter.Shared.Architecture;

public interface IUnitOfWorkFactory
{
    IUnitOfWork Create();
}