using Application.Common;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Security;
using Application.Features.Auth.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthResult>>
{
    private readonly IUserRepository _userRepo;
    private readonly IHasher _hasher;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginQueryHandler(
        IUserRepository userRepo,
        IHasher hasher,
        IJwtTokenGenerator tokenGenerator)
    {
        this._userRepo = userRepo;
        this._hasher = hasher;
        this._tokenGenerator = tokenGenerator;
    }

    public async Task<Result<AuthResult>> Handle(
        LoginQuery request,
        CancellationToken cancellationToken)
    {
        var user = await this._userRepo.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null)
            return new InvalidCredentialsError();
        if (!this._hasher.Verify(request.Password, user.PasswordHash.Value))
            return new InvalidCredentialsError();

        // TODO: Handle multiple roles
        var token = this._tokenGenerator.GenToken(user.Id, [user.Role]);
        return new AuthResult(token);
    }
}
