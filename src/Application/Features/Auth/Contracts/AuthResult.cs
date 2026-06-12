using Application.Features.Users.Contracts;

namespace Application.Features.Auth.Contracts;

public record AuthResult(
    UserResult UserResult,
    Guid SessionId,
    string SessionToken
);
