using FluentValidation;

namespace Application.Features.Users.Commands.ChangeRole;

public class ChangeRoleCommandValidator : AbstractValidator<ChangeRoleCommand>
{
    public ChangeRoleCommandValidator()
    {
        this.RuleFor(x => x.UserId)
            .NotEmpty();

        this.RuleFor(x => x.Role)
            .IsInEnum();
    }
}
