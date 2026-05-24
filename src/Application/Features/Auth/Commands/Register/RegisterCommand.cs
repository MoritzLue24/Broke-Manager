using Application.Features.Auth.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword  // Not really used, just for validation
) : IRequest<Result<AuthResult>>;

// TODO: RegisterCommand Validation