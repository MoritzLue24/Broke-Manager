using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        this.RuleFor(x => x.Password)
            .NotEmpty();

        this.RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password);
    }
}
