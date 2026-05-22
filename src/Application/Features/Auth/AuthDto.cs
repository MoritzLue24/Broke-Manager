namespace Application.Features.Authentification;

public record AuthDto(
    Guid UserId,
    string Token
);
