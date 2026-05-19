using Application.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionHandler : IRequestHandler<GetTransactionQuery, Result<TransactionDto>>
{
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionHandler(ITransactionRepository transactionRepo)
    {
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<TransactionDto>> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var transaction = await this._transactionRepo.GetByIdAsync(
            request.TransactionId,
            cancellationToken);

        if (transaction == null || transaction.UserId != request.UserId)
            return new TransactionNotFoundError(); ;

        return transaction.ToDto();
    }
}
