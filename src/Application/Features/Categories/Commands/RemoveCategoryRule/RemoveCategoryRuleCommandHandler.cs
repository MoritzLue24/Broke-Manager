using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Categories.Commands.RemoveCategoryRule;

// TODO: Use IUserContext
public class RemoveCategoryRuleCommandHandler : IRequestHandler<RemoveCategoryRuleCommand, Result<CategoryResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public RemoveCategoryRuleCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResult>> Handle(
        RemoveCategoryRuleCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;
        var category = await this._categoryRepo.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null || category.UserId != userId)
            return new CategoryNotFoundError();

        var removeResult = category.RemoveRule(request.RuleId);
        if (!removeResult.Success)
            return removeResult.Cast<CategoryResult>();

        await this._uow.SaveChangesAsync(cancellationToken);
        return category.ToResult();
    }
}
