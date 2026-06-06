using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Common;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly ICategoryRepository _categoryRepo;

    public DeleteCategoryCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        ICategoryRepository categoryRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._categoryRepo = categoryRepo;
    }

    public async Task<Result<Unit>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var userId = this._userContext.UserId!.Value;

        var category = await this._categoryRepo.GetByIdAsync(request.Id, cancellationToken);
        if (category is null || category.UserId != userId)
            return new CategoryNotFoundError();

        var domainResult = category.Delete();
        if (!domainResult.Success)
            return domainResult;

        this._categoryRepo.Delete(category);
        await this._uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
