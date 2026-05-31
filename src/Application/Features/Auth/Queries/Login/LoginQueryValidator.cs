using FluentValidation;

namespace Application.Features.Auth.Queries.Login;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        this.RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(255);

        this.RuleFor(x => x.Password)
            .NotEmpty();
    }
}