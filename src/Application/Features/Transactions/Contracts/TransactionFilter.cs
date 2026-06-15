using FluentValidation;

namespace Application.Features.Transactions.Contracts;

public record TransactionFilter(
    Guid[]? TransactionIds,
    Guid[]? CategoryIds,
    DateOnly? From,
    DateOnly? To
);

// Use with .SetValidator(new TransactionFilterValidator())
public class TransactionFilterValidator : AbstractValidator<TransactionFilter>
{
    public TransactionFilterValidator()
    {
        this.RuleFor(x => x.From)
            .NotEmpty()
            .LessThanOrEqualTo(x => x.To)
            .WithMessage("'From' must be before or equal to 'To'.")
            .When(x => x.From is not null);

        this.RuleFor(x => x.To)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.From)
            .WithMessage("'To' must be after or equal to 'From'.")
            .When(x => x.To is not null);
    }
}