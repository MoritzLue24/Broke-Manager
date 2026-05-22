using Application.Common.Interfaces.Persistence;
using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Result<List<TransactionResult>>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetAllTransactionsQueryHandler(ITransactionRepository transactionRepo)
    {
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<List<TransactionResult>>> Handle(
        GetAllTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        var transactions = await this._transactionRepo.GetAllByUserId(request.UserId, cancellationToken);
        return transactions.Select(t => t.ToResult()).ToList();
    }
}
