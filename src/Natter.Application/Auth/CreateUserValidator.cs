

namespace Natter.Application.Auth;

using FluentValidation;

public class CreateUserValidator : AbstractValidator<CreateUser.CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(u => u.Username)
            .Matches("[a-zA-Z][a-zA-Z0-9]{1,29}");

        RuleFor(u => new { u.Password, u.ConfirmPassword })
            .Must(u => u.Password == u.ConfirmPassword)
            .WithMessage("Password confirmation does not match.");
    }
}
