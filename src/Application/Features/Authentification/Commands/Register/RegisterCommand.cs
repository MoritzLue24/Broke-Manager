using MediatR;
using Domain.Common;

namespace Application.Features.Authentification.Commands.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string ConfirmPassword  // Not really used, just for validation
) : IRequest<Result<AuthentificationDto>>;