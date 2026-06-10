using Domain.Enums;
using FluentValidation;

namespace Application.Features.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator()
    {
        this.RuleFor(x => x.Amount)
            .GreaterThan(0)
            .When(x => x.Amount is not null);

        this.RuleFor(x => x.Type)
            .IsEnumName(typeof(TransactionType), caseSensitive: false)
            .When(x => x.Type is not null);

        this.RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(255)
            .When(x => x.Title is not null);

        this.RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);

        this.RuleFor(x => x.CounterParty)
            .MaximumLength(255)
            .When(x => x.CounterParty is not null);
    }
}
