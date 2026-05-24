using FluentValidation;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        this.RuleFor(x => x.Amount)
            .GreaterThan(0);
        // TODO: validate all properties
    }
}
