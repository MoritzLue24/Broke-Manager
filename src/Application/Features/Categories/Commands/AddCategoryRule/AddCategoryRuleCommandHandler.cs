using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Contracts;
using Application.Features.Categories.Interfaces;
using Domain.Common;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Categories.Commands.AddCategoryRule;

public class AddCategoryRuleCommandHandler : IRequestHandler<AddCategoryRuleCommand, Result<CategoryResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public AddCategoryRuleCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResult>> Handle(
        AddCategoryRuleCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;
        var category = await this._categoryRepo.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null || category.UserId != userId)
            return new CategoryNotFoundError();

        var ruleResult = MatchingRule.Create(request.Keyword);
        if (!ruleResult.Success)
            return ruleResult.Cast<CategoryResult>();

        var addResult = category.AddRule(ruleResult.Value);
        if (!addResult.Success)
            return addResult.Cast<CategoryResult>();

        await this._uow.SaveChangesAsync(cancellationToken);
        return category.ToResult();
    }
}
