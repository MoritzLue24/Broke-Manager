namespace Application.Features.Auth.Common;

public record AuthResult(
    Guid UserId,
    string Token
);
