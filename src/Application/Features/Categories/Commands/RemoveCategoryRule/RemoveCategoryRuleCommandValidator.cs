using FluentValidation;

namespace Application.Features.Categories.Commands.RemoveCategoryRule;

public class RemoveCategoryRuleCommandValidator : AbstractValidator<RemoveCategoryRuleCommand>
{
    public RemoveCategoryRuleCommandValidator()
    {
        this.RuleFor(x => x.CategoryId)
            .NotEmpty();

        this.RuleFor(x => x.Keyword)
            .NotEmpty();
    }
}
