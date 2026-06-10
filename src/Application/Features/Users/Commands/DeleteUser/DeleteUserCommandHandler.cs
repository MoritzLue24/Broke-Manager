using Application.Common;
using Application.Common.Interfaces.Persistence;
using Domain.Common;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Users.Commands.DeleteUser;

// TODO: Use IUserContext
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<Unit>>
{
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;

    public DeleteUserCommandHandler(
        IUnitOfWork uow,
        IUserRepository userRepo)
    {
        this._uow = uow;
        this._userRepo = userRepo;
    }

    public async Task<Result<Unit>> Handle(
        DeleteUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this._userRepo.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            return new UserNotFoundError();

        this._userRepo.Delete(user);

        await this._uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
