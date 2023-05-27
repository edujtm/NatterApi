namespace Natter.Domain.Repositories;

using Natter.Domain.Entities;

public interface ISpaceRepository
{
    Task CreateSpace(Space space);
}
