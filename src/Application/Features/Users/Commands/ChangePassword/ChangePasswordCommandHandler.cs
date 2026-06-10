using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Domain.Common;
using Domain.ValueObjects;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Users.Commands.ChangePassword;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<Unit>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;

    public ChangePasswordCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        IUserRepository userRepo,
        IHasher hasher)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._userRepo = userRepo;
        this._hasher = hasher;
    }

    public async Task<Result<Unit>> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;
        var user = await this._userRepo.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return new UserNoLongerExistsError();

        if (!this._hasher.Verify(request.CurrentPassword, user.PasswordHash.Value))
            return new InvalidCredentialsError();

        var hashResult = Hash.Create(this._hasher.Hash(request.NewPassword));
        if (!hashResult.Success)
            return hashResult.Cast<Unit>();

        var changeResult = user.ChangePasswordHash(hashResult.Value);
        if (!changeResult.Success)
            return changeResult.Cast<Unit>();

        await this._uow.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
