using Microsoft.AspNetCore.Mvc;

using Natter.Application.Spaces;

namespace Natter.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SpacesController : Controller
{

    private CreateSpace _createSpace;

    public SpacesController(
        CreateSpace createSpace
    )
    {
        _createSpace = createSpace;
    }

    [HttpPost]
    public IActionResult CreateSpace(CreateSpace.Request request)
    {
        return Ok(_createSpace.Handle(request));
    }
}