using Dapper;
using Natter.Shared.Architecture;
using Natter.Domain.Entities;
using Natter.Domain.Repositories;

namespace Natter.Infrastructure.DbAccess.Repositories;


public class SpaceRepository : ISpaceRepository
{
    private readonly IConnectionFactory _connFactory;

    public SpaceRepository(IConnectionFactory factory)
    {
        _connFactory = factory;
    }

    public async Task CreateSpace(Space space)
    {
        var connection = _connFactory.GetConnection();

        var spaceId = await connection.QuerySingleAsync<int>("SELECT nextval('space_id_seq');");

        await connection.ExecuteAsync(
            @"INSERT INTO spaces(space_id, name, owner)
                VALUES (@SpaceId, @SpaceName, @Owner);
            ",
            new
            {
                SpaceId = spaceId,
                SpaceName = space.Name,
                Owner = space.Owner
            }
        );

        space.Id = spaceId;
    }
}