using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Features.Users.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Users.Commands.ChangeRole;

public class ChangeRoleCommandHandler : IRequestHandler<ChangeRoleCommand, Result<UserResult>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;

    public ChangeRoleCommandHandler(
        IUnitOfWork uow,
        IUserRepository userRepo)
    {
        this._uow = uow;
        this._userRepo = userRepo;
    }

    public async Task<Result<UserResult>> Handle(
        ChangeRoleCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this._userRepo.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
            return new UserNotFoundError();

        var changeResult = user.ChangeRole(request.Role);
        if (!changeResult.Success)
            return changeResult.Cast<UserResult>();

        await this._uow.SaveChangesAsync(cancellationToken);
        return user.ToResult();
    }
}
