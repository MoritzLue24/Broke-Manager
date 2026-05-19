using Microsoft.AspNetCore.Mvc;
using MapsterMapper;
using MediatR;
using Application.Features.Authentification.Commands.Register;
using Contracts.Features.Authentification;
using Api.Errors;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public AuthenticationController(IMediator mediator, IMapper mapper)
    {
        this._mediator = mediator;
        this._mapper = mapper;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthentificationResponse>> Register(
        [FromBody] RegisterRequest request)
    {
        var command = this._mapper.Map<RegisterCommand>(request);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<AuthentificationResponse>>(
            dto => this.Ok(this._mapper.Map<AuthentificationResponse>(dto)),
            errors => errors.ToProblem(this)
        );
    }
}