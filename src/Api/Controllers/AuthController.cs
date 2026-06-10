using Api.Errors;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Queries.Login;
using Contracts.Features.Auth;
using Contracts.Features.Users;
using Infrastructure.Security;
using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly JwtSettings _jwtSettings;

    public AuthController(IMediator mediator, IMapper mapper, IOptions<JwtSettings> jwtSettings)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._jwtSettings = jwtSettings.Value;
    }

    private void SetAuthCookie(string token)
        => this.Response.Cookies.Append(
            this._jwtSettings.CookieName,
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(this._jwtSettings.ExpiryMinutes)
            }
        );

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(
        [FromBody] RegisterRequest request)
    {
        var command = this._mapper.Map<RegisterCommand>(request);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserResponse>>(
            authResult =>
            {
                this.SetAuthCookie(authResult.Token);
                // TODO: Change to created at action
                return this.Created(
                    string.Empty,
                    this._mapper.Map<UserResponse>(authResult)
                );
            },
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request)
    {
        var query = this._mapper.Map<LoginQuery>(request);
        var result = await this._mediator.Send(query);

        return result.Match<IActionResult>(
            authResult =>
            {
                this.SetAuthCookie(authResult.Token);
                return this.Ok(this._mapper.Map<UserResponse>(authResult));
            },
            errors => errors.ToProblem(this)
        );
    }
}
