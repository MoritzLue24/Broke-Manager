using FluentValidation;

namespace Application.Features.Users.Queries.GetUser;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        this.RuleFor(x => x.Id)
            .NotEmpty();
    }
}
