using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.AutoAssign.Contracts;
using Application.Features.AutoAssign.Services;
using Application.Features.Categories.Interfaces;
using Application.Features.Transactions.Contracts;
using Application.Features.Transactions.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Features.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Result<AutoAssignResult>>
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

    public async Task<Result<AutoAssignResult>> Handle(
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
            if (!await this._categoryRepo.ExistsForUserAsync(userId, request.CategoryId.Value, cancellationToken))
                return new CategoryNotFoundError();
            categoryId = request.CategoryId.Value;
            categorySource = CategorySource.Manual;
        }
        // No category specified
        else
        {
            // Get default category
            Guid? categoryIdRes = await this._categoryRepo.GetDefaultIdByUserIdAsync(userId, cancellationToken);
            if (categoryIdRes is null)
                return new DefaultCategoryNotFoundError();

            // Temporarly set category to default category, use this id later for in auto-categorization
            categoryId = categoryIdRes.Value;
            categorySource = CategorySource.Unmatched;
        }

        // Transaction type parsing
        if (!Enum.TryParse<TransactionType>(request.Type, ignoreCase: true, out var transactionType))
            throw new InvalidOperationException();  // Because we assume the request is valid (its validated)

        var domainResult = Transaction.Create(
            userId,
            // TODO: Auto-categorize?
            categoryId,
            categorySource,
            request.Amount,
            transactionType,
            request.Date,
            request.Title,
            request.Description,
            request.CounterParty
        );
        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<AutoAssignResult>();

        var transaction = domainResult.Value;
        var result = new AutoAssignResult(transaction.ToResult(), null);

        // Auto assign when no category specified
        if (!request.CategoryId.HasValue)
        {
            var categories = await this._categoryRepo.GetAllByUserIdAsync(userId, cancellationToken);
            var match = AutoAssignService.FindMatch(transaction, categories, transaction.CategoryId);

            if (match.CategoryId != transaction.CategoryId)
            {
                var changeCategoryResult = transaction.ChangeCategory(match.CategoryId, match.CategorySource);
                if (!changeCategoryResult.Success)
                    return changeCategoryResult.Cast<AutoAssignResult>();                
            }

            if (match.ConflictingCategories is not null)
                result = new(transaction.ToResult(), match.ConflictingCategories);
            else
                result = new(transaction.ToResult(), null);
        }

        this._transactionRepo.Add(domainResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);
        return result;
    }
}
