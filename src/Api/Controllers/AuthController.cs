using Api.Errors;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries.Login;
using Contracts.Features.Auth;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthController(IMediator mediator, IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(
        [FromBody] RegisterRequest request)
    {
        var command = this._mapper.Map<RegisterCommand>(request);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<AuthResponse>>(
            // TODO: Change to created at action
            authResult => this.Created(string.Empty, this._mapper.Map<AuthResponse>(authResult)),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(
        [FromBody] LoginRequest request)
    {
        var query = this._mapper.Map<LoginQuery>(request);
        var result = await this._mediator.Send(query);

        return result.Match<ActionResult<AuthResponse>>(
            authResult => this.Ok(this._mapper.Map<AuthResponse>(authResult)),
            errors => errors.ToProblem(this)
        );
    }
}
