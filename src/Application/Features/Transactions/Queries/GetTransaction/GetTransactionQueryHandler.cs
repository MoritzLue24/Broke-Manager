using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, Result<TransactionResult>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionQueryHandler(ITransactionRepository transactionRepo)
    {
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<TransactionResult>> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var transaction = await this._transactionRepo.GetByIdAsync(
            request.TransactionId,
            cancellationToken);

        if (transaction == null || transaction.UserId != request.UserId)
            return new TransactionNotFoundError(); ;

        return transaction.ToResult();
    }
}
