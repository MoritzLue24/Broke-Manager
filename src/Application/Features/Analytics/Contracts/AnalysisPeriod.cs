using FluentValidation;

namespace Application.Features.Analytics.Contracts;

public record AnalyticsPeriod(
    string Range,
    DateOnly? From,
    DateOnly? To
);

public enum AnalyticsPeriodRange
{
    Custom,
    Last30Days,
    Last90Days,
    ThisMonth,
    ThisYear,
    AllTime
}

public class AnalyticsPeriodValidator : AbstractValidator<AnalyticsPeriod>
{
    public AnalyticsPeriodValidator()
    {
        this.RuleFor(x => x.Range)
            .NotNull()
            .IsEnumName(typeof(AnalyticsPeriodRange), caseSensitive: false);

        this.RuleFor(x => x.To)
            .GreaterThanOrEqualTo(x => x.From)
            .When(x => x.From is not null);

        this.RuleFor(x => x.From)
            .LessThanOrEqualTo(x => x.To)
            .When(x => x.To is not null);
    }
}