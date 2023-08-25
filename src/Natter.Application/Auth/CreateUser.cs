
namespace Natter.Application.Auth;

using Microsoft.AspNetCore.Identity;
using Natter.Domain.Entities;
using Natter.Shared.Architecture;

public class CreateUser : IUseCase
{

    public record CreateUserRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }

    private readonly IUnitOfWorkFactory _uowFactory;
    private readonly UserManager<NatterUser> _userManager;

    public CreateUser(IUnitOfWorkFactory uowFactory, UserManager<NatterUser> userManager)
    {
        _uowFactory = uowFactory;
        _userManager = userManager;
    }

    public async Task Handle(CreateUserRequest request)
    {
        using var uow = _uowFactory.Create();
        var user = await _userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            user = new NatterUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Username,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (result == IdentityResult.Success)
            {
                await uow.CommitAsync();
            }
        }
    }
}
