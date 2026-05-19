using MediatR;
using Domain.Common;

namespace Application.Features.Transactions.Queries.GetTransaction;

public record GetTransactionQuery(
    Guid UserId,
    Guid TransactionId
) : IRequest<Result<TransactionDto>>;