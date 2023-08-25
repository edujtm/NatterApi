namespace Natter.Application.Spaces;
using FluentValidation;

public class CreateSpaceValidator : AbstractValidator<CreateSpace.CreateSpaceRequest>
{
    public CreateSpaceValidator()
    {
        RuleFor(r => r.Name)
            .MaximumLength(255).WithMessage("Space name too long");

        RuleFor(r => r.Owner)
            .Matches("^[a-zA-z][a-zA-Z0-9]{1,29}$").WithMessage("Invalid username: {PropertyValue}");
    }
}
