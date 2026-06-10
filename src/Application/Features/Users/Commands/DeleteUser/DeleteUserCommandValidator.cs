using FluentValidation;

namespace Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty();
    }
}
