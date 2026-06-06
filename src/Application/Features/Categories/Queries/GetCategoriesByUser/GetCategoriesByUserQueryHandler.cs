using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Categories.Common;
using Application.Features.Transactions.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Categories.Queries.GetCategoriesByUser;

public class GetCategoriesByUserQueryHandler : IRequestHandler<GetCategoriesByUserQuery, Result<List<CategoryResult>>>
{
    private readonly IUserContext _userContext;
    private readonly ICategoryRepository _categoryRepo;

    public GetCategoriesByUserQueryHandler(
        IUserContext userContext,
        ICategoryRepository transactionRepo)
    {
        this._userContext = userContext;
        this._categoryRepo = transactionRepo;
    }

    public async Task<Result<List<CategoryResult>>> Handle(
        GetCategoriesByUserQuery request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;

        var categories = await this._categoryRepo.GetAllByUserIdAsync(userId, cancellationToken);
        return categories.Select(t => t.ToResult()).ToList();
    }
}
