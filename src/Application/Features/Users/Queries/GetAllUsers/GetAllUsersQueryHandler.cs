using Application.Features.Users.Contracts;
using Application.Features.Users.Interfaces;
using Domain.Common;
using MediatR;

namespace Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserResult>>>
{
    private readonly IUserRepository _userRepo;

    public GetAllUsersQueryHandler(
        IUserRepository userRepo)
    {
        this._userRepo = userRepo;
    }

    public async Task<Result<List<UserResult>>> Handle(
        GetAllUsersQuery _,
        CancellationToken cancellationToken)
    {
        var users = await this._userRepo.GetAllUsersAsync(cancellationToken);
        return users.Select(u => u.ToResult()).ToList();
    }
}
