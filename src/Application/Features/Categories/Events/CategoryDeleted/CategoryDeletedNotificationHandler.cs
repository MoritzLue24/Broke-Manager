using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using MediatR;

namespace Application.Features.Categories.Events.CategoryDeleted;

public class CategoryDeletedNotificationHandler : INotificationHandler<CategoryDeletedNotification>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ITransactionRepository _transactionRepo;
    private readonly ICategoryRepository _categoryRepo;

    public CategoryDeletedNotificationHandler(
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

    public async Task Handle(
        CategoryDeletedNotification notification,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;

        var transactions = await this._transactionRepo.GetAllByCategoryIdAsync(notification.CategoryId, cancellationToken);
        var defaultCategoryId = await this._categoryRepo.GetDefaultIdByUserIdAsync(userId, cancellationToken)
            // TODO: Custom exception
            ?? throw new InvalidOperationException("Default category not found");

        foreach (var transaction in transactions)
        {
            if (transaction.UserId != userId)
                continue;
            var domainResult = transaction.ResetCategory(defaultCategoryId);
            if (!domainResult.Success)
                // TODO: Custom exception
                throw new InvalidOperationException(domainResult.Errors.ToString());
        }

        await this._uow.SaveChangesAsync(cancellationToken);
    }
}
