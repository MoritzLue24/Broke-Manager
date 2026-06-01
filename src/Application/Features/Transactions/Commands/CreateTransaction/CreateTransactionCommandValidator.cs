using FluentValidation;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        this.RuleFor(x => x.Amount)
            .NotEmpty()
            .GreaterThan(0);

        this.RuleFor(x => x.Type)
            .IsInEnum();

        this.RuleFor(x => x.Date)
            .NotEmpty();

        this.RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(255);

        this.RuleFor(x => x.Description)
            .NotNull()
            .MaximumLength(500);

        this.RuleFor(x => x.CounterParty)
            .NotNull()
            .MaximumLength(255);
    }
}
