using Application.Common.Interfaces.Persistence;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public class GetTransactionsByUserHandler : IRequestHandler<GetTransactionsByUserQuery, Result<List<TransactionDto>>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionsByUserHandler(ITransactionRepository transactionRepo)
    {
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<List<TransactionDto>>> Handle(
        GetTransactionsByUserQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await this._transactionRepo.GetAllByUserId(request.UserId, cancellationToken);
        return transactions.Select(t => t.ToDto()).ToList();
    }
}
