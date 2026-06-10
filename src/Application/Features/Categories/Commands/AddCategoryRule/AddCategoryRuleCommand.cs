using Application.Common.Interfaces.Security;
using Application.Features.Categories.Common;
using Domain.Common;
using Domain.Enums;
using MediatR;

namespace Application.Features.Categories.Commands.AddCategoryRule;

/// <summary>
/// Adds a new user to the database, 
/// creates a default category and generates a jwt.
/// </summary>
/// 
/// <param name="CategoryId">The users email, in valid email format</param>
/// <param name="Keyword">Password with at least 8 chars, one letter, one digit and one punctuation</param>
/// 
/// <returns>
/// CategoryResult on success.
/// On failure:
/// <list type="bullet">
///     <item>InvalidEmailFormatError - When Email format invalid</item>
///     <item>InvalidHashFormatError - When Hash format invalid</item>
///     <item>UserAlreadyExistsError - When a user with this email is already registered</item>
/// </list>
/// </returns>
public record AddCategoryRuleCommand(
    Guid CategoryId,
    string Keyword
) : IRequest<Result<CategoryResult>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
