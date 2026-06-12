using Application.Features.Users.Contracts;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

/// <summary>
/// Adds a new user to the database, 
/// creates a default category.
/// </summary>
/// 
/// <param name="Email">The users email, in valid email format</param>
/// <param name="Password">Password with at least 8 chars, one letter, one digit and one punctuation</param>
/// <param name="ConfirmPassword">Must equal `Password`</param>
/// 
/// <returns>
/// UserResult on success.
/// On failure:
/// <list type="bullet">
///     <item>InvalidEmailFormatError - When Email format invalid</item>
///     <item>InvalidHashFormatError - When Hash format invalid</item>
///     <item>UserAlreadyExistsError - When a user with this email is already registered</item>
/// </list>
/// </returns>
public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword  // Not really used, just for validation
) : IRequest<Result<UserResult>>;
