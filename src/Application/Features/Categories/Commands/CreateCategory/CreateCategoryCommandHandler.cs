using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Contracts;
using Application.Features.Categories.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CategoryResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public CreateCategoryCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResult>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;

        if (await this._categoryRepo.NameExistsForUserAsync(userId, request.Name, cancellationToken))
            return new CategoryNameAlreadyExistsError();

        var domainResult = Category.Create(
            userId,
            request.Name,
            false
        );

        // On failure, map the domain error to an application error
        // For now, the errors are basically the same but we dont want to
        // pass domain errors into the Api layer
        if (!domainResult.Success)
            return domainResult.Cast<CategoryResult>();

        // TODO: Use matching rules in request object
        foreach (var keyword in request.Keywords)
        {
            var ruleResult = MatchingRule.Create(keyword);
            if (!ruleResult.Success)
                return ruleResult.Cast<CategoryResult>();

            domainResult.Value.AddRule(ruleResult.Value);
        }

        this._categoryRepo.Add(domainResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);
        return domainResult.Cast(t => t.ToResult());
    }
}
