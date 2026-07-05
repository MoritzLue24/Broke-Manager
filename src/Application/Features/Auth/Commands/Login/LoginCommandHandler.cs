using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Contracts;
using Application.Features.Auth.Interfaces;
using Application.Features.Users.Contracts;
using Application.Features.Users.Interfaces;
using Domain.Common;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResult>>
{
    private readonly IUnitOfWork _uow;
    private readonly ISessionRepository _sessionRepo;
    private readonly IUserRepository _userRepo;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IHasher _hasher;
    private readonly ISessionSettings _sessionSettings;

    public LoginCommandHandler(
        IUnitOfWork uow,
        ISessionRepository sessionRepo,
        IUserRepository userRepo,
        ITokenGenerator tokenGenerator,
        IHasher hasher,
        ISessionSettings sessionSettings)
    {
        this._uow = uow;
        this._sessionRepo = sessionRepo;
        this._userRepo = userRepo;
        this._hasher = hasher;
        this._tokenGenerator = tokenGenerator;
        this._sessionSettings = sessionSettings;
    }

    public async Task<Result<AuthResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await this._userRepo.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null ||
            !this._hasher.Verify(
                request.Password,
                user.PasswordHash.Value
            ))
            return new InvalidCredentialsError();

        var token = this._tokenGenerator.GenToken();
        var hashResult = Hash.Create(this._hasher.Hash(token));
        if (!hashResult.Success)
            return hashResult.Cast<AuthResult>();

        var sessionResult = Session.Create(
            user.Id,
            [user.Role], // TODO: multiple roles
            hashResult.Value,
            DateTime.UtcNow.AddHours(this._sessionSettings.ExpiryHours)
        );
        if (!sessionResult.Success)
            return sessionResult.Cast<AuthResult>();

        this._sessionRepo.Add(sessionResult.Value);
        await this._uow.SaveChangesAsync(cancellationToken);

        // Delete oldest session if capacity is reached
        if (await this._sessionRepo.CountActiveByUserAsync(user.Id, cancellationToken) >
            this._sessionSettings.MaxSessionsPerUser)
            await this._sessionRepo.ExecuteDeleteMostInactiveByUser(user.Id, cancellationToken);

        return new AuthResult(
            user.ToResult(),
            sessionResult.Value.Id,
            token
        );
    }
}
