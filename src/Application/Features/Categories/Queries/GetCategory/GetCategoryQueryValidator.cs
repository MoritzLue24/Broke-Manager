using FluentValidation;

namespace Application.Features.Categories.Queries.GetCategory;

public class GetTransactionQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetTransactionQueryValidator()
    {
        this.RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
