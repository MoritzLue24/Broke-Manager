using Application.Common;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransaction;

public class GetTransactionQueryHandler : IRequestHandler<GetTransactionQuery, Result<TransactionResult>>
{
    private readonly IUserContext _userContext;
    private readonly ITransactionRepository _transactionRepo;

    public GetTransactionQueryHandler(IUserContext userContext, ITransactionRepository transactionRepo)
    {
        this._userContext = userContext;
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<TransactionResult>> Handle(
        GetTransactionQuery request,
        CancellationToken cancellationToken)
    {
        var transaction = await this._transactionRepo.GetByIdAsync(
            request.TransactionId,
            cancellationToken);

        if (transaction is null || transaction.UserId != this._userContext.UserId)
            return new TransactionNotFoundError(); ;

        return transaction.ToResult();
    }
}
