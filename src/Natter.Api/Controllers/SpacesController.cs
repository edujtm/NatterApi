namespace Natter.Api.Controllers;
using Microsoft.AspNetCore.Mvc;

using Natter.Application.Spaces;

[ApiController]
[Route("[controller]")]
public class SpacesController : Controller
{

    private readonly CreateSpace _createSpace;

    public SpacesController(
        CreateSpace createSpace
    ) => _createSpace = createSpace;

    [HttpPost]
    public async Task<IActionResult> CreateSpace(CreateSpace.CreateSpaceRequest request)
        => Ok(await _createSpace.Handle(request));
}
