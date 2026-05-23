namespace Api.Errors;

public static class ErrorTypes
{
    private const string Base = "urn:broke-manager:errors";
    public const string Internal = $"{Base}:internal-server-error";
    public const string Validation = $"{Base}:validation";
    public const string Unauthorized = $"{Base}:unauthorized";
    public const string Forbidden = $"{Base}:forbidden";
    public const string CategoryNotFound = $"{Base}:category-not-found";
    public const string DefaultCategoryNotFound = $"{Base}:default-category-not-found";
}
