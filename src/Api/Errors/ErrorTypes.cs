namespace Api.Errors;

public static class ErrorTypes
{
    private const string Base = "urn:broke-manager:errors";
    public const string CategoryNotFound = $"{Base}:category-not-found";
    public const string DefaultCategoryNotFound = $"{Base}:default-category-not-found";
}