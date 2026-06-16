using Application.Features.Analytics.Contracts;
using FluentValidation;

namespace Application.Features.Analytics.Queries.Summary;

public class SummaryQueryValidator : AbstractValidator<SummaryQuery>
{
    public SummaryQueryValidator()
    {
        this.RuleFor(x => x.Period)
            .SetValidator(new AnalyticsPeriodValidator());
    }
}
