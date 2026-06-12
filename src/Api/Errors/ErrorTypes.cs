namespace Api.Errors;

// The string that gets passed in the "type" field of a problem response.
// Used to identify the error, RFC standard
public static class ErrorTypes
{
    private const string Base = "urn:error";

    // Common
    public const string Internal = $"{Base}:internal";
    public const string Validation = $"{Base}:validation";
    public const string NotFound = $"{Base}:not-found";
    public const string Duplicate = $"{Base}:duplicate";

    // Auth
    public const string Unauthorized = $"{Base}:auth:unauthorized";
    public const string Forbidden = $"{Base}:auth:forbidden";
    public const string InvalidCredentials = $"{Base}:auth:invalid-credentials";
    public const string TokenInvalid = $"{Base}:auth:token-invalid";

    // Users
    public const string PropertyAlreadyAssigned = $"{Base}:user:property-already-assigned";

    // Categories
    public const string CategoryIsDefault = $"{Base}:category:is-default";
    public const string DefaultCategoryNotFound = $"{Base}:category:default-not-found";
}
