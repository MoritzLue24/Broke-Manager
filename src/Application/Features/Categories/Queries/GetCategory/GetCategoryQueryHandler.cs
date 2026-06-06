using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Common;
using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategory;

public class GetTransactionQueryHandler : IRequestHandler<GetCategoryQuery, Result<CategoryResult>>
{
    private readonly IUserContext _userContext;
    private readonly ICategoryRepository _categoryRepo;

    public GetTransactionQueryHandler(IUserContext userContext, ICategoryRepository transactionRepo)
    {
        this._userContext = userContext;
        this._categoryRepo = transactionRepo;
    }

    public async Task<Result<CategoryResult>> Handle(
        GetCategoryQuery request,
        CancellationToken cancellationToken)
    {
        var category = await this._categoryRepo.GetByIdAsync(
            request.CategoryId,
            cancellationToken);

        if (category is null || category.UserId != this._userContext.UserId)
            return new CategoryNotFoundError();

        return category.ToResult();
    }
}
