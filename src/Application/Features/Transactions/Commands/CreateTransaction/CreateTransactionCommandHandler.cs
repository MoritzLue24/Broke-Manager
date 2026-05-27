using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

// TODO: Use IUserContext
public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<TransactionResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CreateTransactionCommandHandler(
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
        CreateTransactionCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;
        Guid categoryId;
        CategorySource categorySource;

        // Category specified -> source = Manual
        if (request.CategoryId.HasValue)
        {
            // Check if the given category exists
            if (!await this._categoryRepo.ExistsForUserAsync(userId, request.CategoryId.Value))
                return new CategoryNotFoundError();
            categoryId = request.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified -> (later auto-categorize) -> source = Unmatched with default category
        else
        {
            // Get default category
            Guid? categoryIdRes = await this._categoryRepo.GetDefaultIdByUserIdAsync(userId);
            if (categoryIdRes is null)
                return new DefaultCategoryNotFoundError();
            categoryId = categoryIdRes.Value;
            categorySource = CategorySource.Unmatched;
        }

        var domainResult = Transaction.Create(
            userId,
            // TODO: Auto-categorize?
            categoryId,
            categorySource,
            request.Amount,
            request.Type,
            request.Date,
            request.Title,
            request.Description,
            request.CounterParty
        );
        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<TransactionResult>();

        this._transactionRepo.Add(domainResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);
        return domainResult.Cast(t => t.ToResult());
    }
}
