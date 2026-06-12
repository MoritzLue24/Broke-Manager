using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Interfaces;
using Domain.Common;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<Unit>>
{
    private readonly IUnitOfWork _uow;
    private readonly ISessionRepository _sessionRepo;
    private readonly IUserContext _userContext;

    public LogoutCommandHandler(
        IUnitOfWork uow,
        ISessionRepository sessionRepo,
        IUserContext userContext)
    {
        this._uow = uow;
        this._sessionRepo = sessionRepo;
        this._userContext = userContext;
    }

    public async Task<Result<Unit>> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        var session = await this._sessionRepo.GetByIdAsync(
            this._userContext.SessionId ?? Guid.Empty,
            cancellationToken
        );
        if (session is null)    // Should not happen
            return new UnauthorizedError();

        this._sessionRepo.Delete(session);
        await this._uow.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
