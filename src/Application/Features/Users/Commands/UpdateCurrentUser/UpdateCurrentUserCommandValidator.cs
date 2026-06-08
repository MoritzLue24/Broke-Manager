using FluentValidation;

namespace Application.Features.Users.Commands.UpdateCurrentUser;

public class UpdateCurrentUserCommandValidator : AbstractValidator<UpdateCurrentUserCommand>
{
    public UpdateCurrentUserCommandValidator()
    {
        this.RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => x.Email is not null);
    }
}
