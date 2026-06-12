namespace Application.Common.Interfaces.Security;

public interface ISessionSettings
{
    string CookieName { get; init; }
    int ExpiryHours { get; init; }
}
