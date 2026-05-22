namespace Infrastructure.Security;

public sealed record JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; init; } = null!;
    public string Audience { get; init; } = null!;

    public static bool Validate(JwtSettings settings)
    {
        if (string.IsNullOrEmpty(settings.Secret))
            throw new InvalidOperationException($"{SectionName}__Secret is not set");
        if (string.IsNullOrEmpty(settings.Issuer))
            throw new InvalidOperationException($"{SectionName}__Issuer is not set");
        if (string.IsNullOrEmpty(settings.Audience))
            throw new InvalidOperationException($"{SectionName}__Audience is not set");

        if (settings.ExpiryMinutes <= 0)
            throw new InvalidOperationException($"{SectionName}__ExpiryMinutes must be greater than 0");

        return true;
    }
}