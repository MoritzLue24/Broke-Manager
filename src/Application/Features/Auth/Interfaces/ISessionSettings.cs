namespace Application.Features.Auth.Interfaces;

public interface ISessionSettings
{
    string CookieName { get; init; }
    int ExpiryHours { get; init; }
    int MaxSessionsPerUser { get; init; }
    int LastSeenUpdateWindowMinutes { get; init; }
}
