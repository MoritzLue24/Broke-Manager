using Application.Features.Auth.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Queries.Login;

/// <summary>
/// Gets the user from the database and generates a jwt.
/// </summary>
/// 
/// <param name="Email">The users email, in valid email format</param>
/// <param name="Password">Password with at least 8 chars, one letter, one digit and one punctuation</param>
/// 
/// <returns>
/// AuthResult on success.
/// On failure:
/// <list type="bullet">
///     <item>InvalidCredentialsError - When email or password is incorrect</item>
/// </list>
/// </returns>
public record LoginQuery(
    string Email,
    string Password
) : IRequest<Result<AuthResult>>;

// TODO: LoginCommand Validation
