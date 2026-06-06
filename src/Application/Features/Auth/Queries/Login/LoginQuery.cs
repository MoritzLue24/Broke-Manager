using Application.Features.Auth.Common;
using Domain.Common;
using MediatR;

namespace Application.Features.Auth.Queries.Login;

public record LoginQuery(
    string Email,
    string Password
) : IRequest<Result<LoginResult>>;

// TODO: LoginCommand Validation
