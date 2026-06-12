using Application.Features.Auth.Interfaces;

namespace Infrastructure.Security;

public sealed record SessionSettings : ISessionSettings
{
    public const string SectionName = "SessionSettings";
    public string CookieName { get; init; } = null!;
    public int ExpiryHours { get; init; }
    public int MaxSessionsPerUser { get; init; }
    public int LastSeenUpdateWindowMinutes { get; init; }

    public bool Validate()
    {
        if (string.IsNullOrWhiteSpace(this.CookieName))
            throw new InvalidOperationException($"{SectionName}__CookieName is not set");

        if (this.ExpiryHours <= 0)
            throw new InvalidOperationException($"{SectionName}__ExpiryHours must be greater than 0");

        if (this.MaxSessionsPerUser <= 0)
            throw new InvalidOperationException($"{SectionName}__MaxSessionsPerUser must be greater than 0");

        if (this.LastSeenUpdateWindowMinutes <= 0)
            throw new InvalidOperationException($"{SectionName}__LastSeenUpdateWindowMinutes must be greater than 0");

        return true;
    }
}
