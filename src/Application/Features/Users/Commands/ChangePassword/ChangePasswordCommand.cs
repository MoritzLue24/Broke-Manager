using Application.Common.Interfaces.Security;
using Domain.Common;
using Domain.Enums;
using MediatR;

using Unit = Domain.Common.Unit;

namespace Application.Features.Users.Commands.ChangePassword;

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
) : IRequest<Result<Unit>>, IRequireAuthorization
{
    // Admins and Users can execute this command
    // (not both roles are needed)
    public Role[] Roles => [Role.User, Role.Admin];
}
