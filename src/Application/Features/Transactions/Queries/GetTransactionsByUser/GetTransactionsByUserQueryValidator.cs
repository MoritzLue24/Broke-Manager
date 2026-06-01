using FluentValidation;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionsByUserQueryValidator : AbstractValidator<GetTransactionQuery>
{
    public GetTransactionsByUserQueryValidator()
    {
        
    }
}
