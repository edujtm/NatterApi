
using Natter.Domain.Entities;

namespace Natter.Domain.Repositories;

public interface ISpaceRepository
{
    Task CreateSpace(Space space);
}