using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Contracts;
using Application.Features.Categories.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Features.Categories.Commands.UpdateCategory;

// TODO: Use IUserContext
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<CategoryResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public UpdateCategoryCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<CategoryResult>> Handle(
        UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;
        var category = await this._categoryRepo.GetByIdAsync(request.CategoryId, cancellationToken);

        if (category is null || category.UserId != userId)
            return new CategoryNotFoundError();

        if (category.IsDefault)
            return new CategoryIsDefaultError();

        if (request.Name is not null)
        {
            if (await this._categoryRepo.NameExistsForUserAsync(userId, request.Name, cancellationToken))
                return new CategoryNameAlreadyExistsError();

            var domainResult = category.ChangeName(request.Name);
            if (!domainResult.Success)
                return domainResult.Cast<CategoryResult>();
        }

        await this._uow.SaveChangesAsync(cancellationToken);
        return category.ToResult();
    }
}
