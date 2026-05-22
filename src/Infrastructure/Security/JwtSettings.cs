namespace Infrastructure.Security;

public sealed record JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;
    public string CookieName { get; init; } = null!;

    public bool Validate()
    {
        if (string.IsNullOrEmpty(this.Secret))
            throw new InvalidOperationException($"{SectionName}__Secret is not set");
        if (string.IsNullOrEmpty(this.Issuer))
            throw new InvalidOperationException($"{SectionName}__Issuer is not set");
        if (string.IsNullOrEmpty(this.Audience))
            throw new InvalidOperationException($"{SectionName}__Audience is not set");
        if (string.IsNullOrEmpty(this.CookieName))
            throw new InvalidOperationException($"{SectionName}__CookieName is not set");

        if (this.ExpiryMinutes <= 0)
            throw new InvalidOperationException($"{SectionName}__ExpiryMinutes must be greater than 0");

        return true;
    }
}
