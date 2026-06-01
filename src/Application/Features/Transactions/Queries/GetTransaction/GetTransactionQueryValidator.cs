using FluentValidation;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionQueryValidator : AbstractValidator<GetTransactionQuery>
{
    public GetTransactionQueryValidator()
    {
        this.RuleFor(x => x.TransactionId)
            .NotEmpty();
    }
}
