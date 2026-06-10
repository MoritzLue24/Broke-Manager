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
            .MinimumLength(8)
            .Must(password => password.Any(c => char.IsLetter(c))).WithMessage("'Password' must contain at least one letter")
            .Must(password => password.Any(c => char.IsDigit(c))).WithMessage("'Password' must contain at least one digit")
            .Must(password => password.Any(c => char.IsPunctuation(c))).WithMessage("'Password' must contain at least one punctuation")
            .NotEmpty();

        this.RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Equal(x => x.Password);
    }
}
