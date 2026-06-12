using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler : IRequestHandler<DeleteTransactionCommand, Result<Unit>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;

    public DeleteTransactionCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ITransactionRepository transactionRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._transactionRepo = transactionRepo;
    }

    public async Task<Result<Unit>> Handle(
        DeleteTransactionCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;

        var transaction = await this._transactionRepo.GetByIdAsync(request.Id, cancellationToken);
        if (transaction is null || transaction.UserId != userId)
            return new CategoryNotFoundError();

        var domainResult = transaction.Delete();
        if (!domainResult.Success)
            return domainResult;

        this._transactionRepo.Delete(transaction);
        await this._uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
