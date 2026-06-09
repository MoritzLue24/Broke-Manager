using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Common;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Users.Commands.DeleteCurrentUser;

// TODO: Use IUserContext
public class DeleteCurrentUserCommandHandler : IRequestHandler<DeleteCurrentUserCommand, Result<Unit>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;

    public DeleteCurrentUserCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        IUserRepository userRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._userRepo = userRepo;
    }

    public async Task<Result<Unit>> Handle(
        DeleteCurrentUserCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;
        var user = await this._userRepo.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return new UserNotFoundError();

        this._userRepo.Delete(user);

        await this._uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
