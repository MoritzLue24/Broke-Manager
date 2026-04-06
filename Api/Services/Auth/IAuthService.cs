using Api.DTOs.Auth;
using Api.Exceptions;


namespace Api.Services.Auth
{
    public interface IAuthService
    {
        /// <summary>
        /// Adds a new user to the database, if a user with this email doesnt exist already
        /// </summary>
        /// <param name="registerDto">The register request (we assume the passwords match!)</param>
        /// <returns>An object containing everything thats passed to the JWT</returns>
        /// <exception cref="AlreadyExistsException">If the user already exists</exception>
        Task<UserClaimsDto> RegisterUserAsync(RegisterRequestDto registerDto);
        /// <summary>
        /// Checks if a user with specified email exists, and if the password is corrent.
        /// Then returns a corresponding UserClaimsDto that can be used to create a JWT (or null)
        /// </summary>
        /// <param name="loginDto">The password + email</param>
        /// <returns>A ClaimsDto containing everything thats passed to the JWT OR null if the email / password is incorrect</returns>
        Task<UserClaimsDto?> ValidateUserAsync(LoginRequestDto loginDto);
        /// <summary>
        /// Generates a JWT with the given claims, and the config specified in a appsettings.json.
        /// </summary>
        /// <param name="userClaims">The claims that are contained in the JWT.</param>
        /// <returns>The JWT as string</returns>
        /// <exception cref="MissingConfigurationException">If any of the following keys are not found in appsettings.json:
        /// Jwt:Key (nur ASCII), Jwt:ExpiresInMinutes, Jwt:Issuer, Jwt:Audience</exception>
        /// <exception cref="FormatException">If Jwt:ExpiresInMinutes does not have the correct int-format</exception>
        string GenJwtToken(UserClaimsDto userClaims);
    }
}