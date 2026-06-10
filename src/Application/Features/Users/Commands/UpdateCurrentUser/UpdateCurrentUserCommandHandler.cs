using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Transactions.Common;
using Application.Features.Users.Common;
using Domain.Common;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Users.Commands.UpdateCurrentUser;

// TODO: Use IUserContext
public class UpdateCurrentUserCommandHandler : IRequestHandler<UpdateCurrentUserCommand, Result<UserResult>>
{
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _uow;
    private readonly IUserRepository _userRepo;

    public UpdateCurrentUserCommandHandler(
        IUserContext userContext,
        IUnitOfWork uow,
        IUserRepository userRepo)
    {
        this._userContext = userContext;
        this._uow = uow;
        this._userRepo = userRepo;
    }

    public async Task<Result<UserResult>> Handle(
        UpdateCurrentUserCommand request,
        CancellationToken cancellationToken)
    {
        Guid userId = this._userContext.UserId!.Value;
        var user = await this._userRepo.GetByIdAsync(userId, cancellationToken);

        if (user is null)
            return new UserNoLongerExistsError();

        if (request.Email is not null)
        {
            var emailResult = Email.Create(request.Email);
            if (!emailResult.Success)
                return emailResult.Cast<UserResult>();

            var changeResult = user.ChangeEmail(emailResult.Value);
            if (!changeResult.Success)
                return changeResult.Cast<UserResult>();
        }

        await this._uow.SaveChangesAsync(cancellationToken);
        return user.ToResult();
    }
}
