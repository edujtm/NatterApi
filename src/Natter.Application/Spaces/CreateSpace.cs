namespace Natter.Application.Spaces;
using FluentValidation;
using Natter.Domain.Entities;
using Natter.Domain.Repositories;
using Natter.Shared.Architecture;

public class CreateSpace : IUseCase
{
    public record CreateSpaceRequest
    {
        public string? Name { get; set; }
        public string? Owner { get; set; }
    }

    public record Response
    {
        public int SpaceId { get; set; }
        public string? SpaceName { get; set; }
    }

    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly ISpaceRepository _spaceRepository;

    public CreateSpace(IUnitOfWorkFactory factory, ISpaceRepository spaceRepository)
    {
        _spaceRepository = spaceRepository;
        _uowFactory = factory;
    }

    public async Task<Response> Handle(CreateSpaceRequest request)
    {
        var validator = new CreateSpaceValidator();

        validator.ValidateAndThrow(request);
        var result = validator.Validate(request);

        var space = new Space
        {
            Name = request.Name,
            Owner = request.Owner,
        };

        using var uow = _uowFactory.Create();

        await _spaceRepository.CreateSpace(space);
        await uow.CommitAsync();

        return new Response
        {
            SpaceId = space.Id,
            SpaceName = space.Name,
        };
    }
}
