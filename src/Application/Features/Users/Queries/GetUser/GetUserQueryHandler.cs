using Application.Common;
using Application.Features.Users.Contracts;
using Application.Features.Users.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, Result<UserResult>>
{
    private readonly IUserRepository _userRepo;

    public GetUserQueryHandler(
        IUserRepository userRepo)
    {
        this._userRepo = userRepo;
    }

    public async Task<Result<UserResult>> Handle(
        GetUserQuery request,
        CancellationToken cancellationToken)
    {
        var user = await this._userRepo.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            return new UserNotFoundError();

        return user.ToResult();
    }
}
