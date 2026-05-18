using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public class GetTransactionsByUserHandler : IRequestHandler<GetTransactionsByUserQuery, Result<List<TransactionDto>>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionsByUserHandler(ITransactionRepository transactionRepo)
        => _transactionRepo = transactionRepo;

    public async Task<Result<List<TransactionDto>>> Handle(GetTransactionsByUserQuery query, CancellationToken ct)
    {
        var transactions = _transactionRepo.GetAllByUserId(query.UserId);
        return transactions.Select(t => t.ToDto()).ToList();
    }
}