using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, Result<List<TransactionResult>>>
{
    private readonly IUserContext _userContext;
    private readonly ITransactionRepository _transactionRepo;

    public GetAllTransactionsQueryHandler(
        IUserContext userContext,
        ITransactionRepository transactionRepo)
    {
        this._userContext = userContext;
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<List<TransactionResult>>> Handle(
        GetAllTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;

        var transactions = await this._transactionRepo.GetAllByUserId(userId, cancellationToken);
        return transactions.Select(t => t.ToResult()).ToList();
    }
}
