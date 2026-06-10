using Application.Features.Users.Common;

namespace Application.Features.Auth.Common;

public record AuthResult(
    UserResult UserResult,
    string Token
);
