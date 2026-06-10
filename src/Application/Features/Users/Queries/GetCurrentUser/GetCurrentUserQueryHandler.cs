using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Users.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Users.Queries.GetCurrentUser;

public class GetCurrenUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<UserResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepo;

    public GetCurrenUserQueryHandler(
        IUserContext userContext,
        IUserRepository userRepo)
    {
        this._userContext = userContext;
        this._userRepo = userRepo;
    }

    public async Task<Result<UserResult>> Handle(
        GetCurrentUserQuery _,
        CancellationToken cancellationToken)
    {
        var me = await this._userRepo.GetByIdAsync(this._userContext.UserId!.Value, cancellationToken);

        if (me is null)
            return new UserNoLongerExistsError();

        return me.ToResult();
    }
}
