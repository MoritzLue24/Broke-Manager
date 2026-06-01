using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Transactions.Queries.GetTransactionsByUser;

public record GetTransactionsByUserQuery(   // TODO: with pages etc.
) : IRequest<Result<List<TransactionResult>>>;
