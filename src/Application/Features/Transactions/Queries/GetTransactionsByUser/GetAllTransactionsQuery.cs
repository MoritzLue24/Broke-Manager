using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public record GetAllTransactionsQuery(   // TODO: with pages etc.
    Guid UserId
) : IRequest<Result<List<TransactionResult>>>;
