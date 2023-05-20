using Natter.Shared.Architecture;

namespace Natter.Application.Spaces;

public class CreateSpace : IUseCase
{
    public record Request
    {
        public string Name { get; set; }
        public string Owner { get; set; }
    }

    public record Response
    {
        public string SpaceId { get; set; }
        public string SpaceName { get; set; }
    }


    public Response Handle(Request request)
    {
        return null;
    }
}