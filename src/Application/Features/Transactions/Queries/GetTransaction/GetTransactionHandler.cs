using Application.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, Result<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionHandler(ITransactionRepository transactionRepo)
        => _transactionRepo = transactionRepo;

    public async Task<Result<TransactionDto>> Handle(GetTransactionQuery query, CancellationToken ct)
    {
        var transaction = await _transactionRepo.GetByIdAsync(query.TransactionId);

        if (transaction == null || transaction.UserId != query.UserId)
            return new TransactionNotFoundError();;

        return transaction.ToDto();
    }
}