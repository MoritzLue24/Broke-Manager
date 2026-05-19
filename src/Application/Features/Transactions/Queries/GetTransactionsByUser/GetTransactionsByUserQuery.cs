using MediatR;
using Domain.Common;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public record GetTransactionsByUserQuery(   // TODO: with pages etc.
    Guid UserId
) : IRequest<Result<List<TransactionDto>>>;