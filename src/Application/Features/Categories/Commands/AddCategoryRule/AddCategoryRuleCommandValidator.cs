using FluentValidation;

namespace Application.Features.Categories.Commands.AddCategoryRule;

public class AddCategoryRuleCommandValidator : AbstractValidator<AddCategoryRuleCommand>
{
    public AddCategoryRuleCommandValidator()
    {
        this.RuleFor(x => x.CategoryId)
            .NotEmpty();

        this.RuleFor(x => x.Keyword)
            .NotEmpty()
            .MaximumLength(255);
    }
}
