using FluentValidation;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public class GetTransactionsByUserQueryValidator : AbstractValidator<GetTransactionsByUserQuery>
{
    public GetTransactionsByUserQueryValidator()
    {
        
    }
}
