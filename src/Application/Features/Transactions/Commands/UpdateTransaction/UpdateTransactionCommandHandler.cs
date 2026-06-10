using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.UpdateTransaction;

// TODO: Use IUserContext
public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Result<TransactionResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public UpdateTransactionCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ITransactionRepository transactionRepo,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._transactionRepo = transactionRepo;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<TransactionResult>> Handle(
        UpdateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;
        var transaction = await this._transactionRepo.GetByIdAsync(request.TransactionId, cancellationToken);

        if (transaction is null || transaction.UserId != userId)
            return new TransactionNotFoundError();

        // Category specified -> source = Manual
        if (request.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await this._categoryRepo.ExistsForUserAsync(userId, request.CategoryId.Value, cancellationToken))
                return new CategoryNotFoundError();

            var domainResult = transaction.ChangeCategory(request.CategoryId.Value, CategorySource.Manual);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.Amount.HasValue)
        {
            var domainResult = transaction.ChangeAmount(request.Amount.Value, transaction.Type);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.Type is not null)
        {
            // Transaction type parsing
            if (!Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var transactionType))
                throw new InvalidOperationException();  // Because we assume the request is valid (its validated)

            var domainResult = transaction.ChangeAmount(transaction.Amount, transactionType);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.Date.HasValue)
        {
            var domainResult = transaction.ChangeDate(request.Date.Value);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.Title is not null)
        {
            var domainResult = transaction.ChangeTitle(request.Title);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.Description is not null)
        {
            var domainResult = transaction.ChangeDescription(request.Description);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        if (request.CounterParty is not null)
        {
            var domainResult = transaction.ChangeCounterParty(request.CounterParty);
            if (!domainResult.Success)
                return domainResult.Cast<TransactionResult>();
        }

        await this._uow.SaveChangesAsync(cancellationToken);
        return transaction.ToResult();
    }
}
