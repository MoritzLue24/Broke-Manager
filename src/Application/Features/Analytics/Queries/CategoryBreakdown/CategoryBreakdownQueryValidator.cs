using Application.Features.Analytics.Contracts;
using FluentValidation;

namespace Application.Features.Analytics.Queries.CategoryBreakdown;

public class CategoryBreakdownQueryValidator : AbstractValidator<CategoryBreakdownQuery>
{
    public CategoryBreakdownQueryValidator()
    {
        this.RuleFor(x => x.Period)
            .SetValidator(new AnalyticsPeriodValidator());
    }
}
