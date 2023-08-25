

namespace Natter.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Natter.Application.Auth;

[ApiController]
[Route("[controller]")]
public class AuthController : Controller
{

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp(
        [FromServices] CreateUser createUser,
        CreateUser.CreateUserRequest request
    )
    {
        await createUser.Handle(request);
        return Ok();
    }
}
