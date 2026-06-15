using Api.Errors;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Logout;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Interfaces;
using Contracts.Features.Auth.Requests;
using Contracts.Features.Users.Responses;
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
    private readonly ISessionSettings _sessionSettings;
    private readonly ISessionCookieParser _sessionCookieParser;

    public AuthController(
        IMediator mediator,
        IMapper mapper,
        ISessionSettings sessionSettings,
        ISessionCookieParser sessionCookieParser)
    {
        this._mediator = mediator;
        this._mapper = mapper;
        this._sessionSettings = sessionSettings;
        this._sessionCookieParser = sessionCookieParser;
    }

    private void SetSessionCookie(Guid sessionId, string sessionToken)
        => this.Response.Cookies.Append(
            this._sessionSettings.CookieName,
            this._sessionCookieParser.CreateCookieValue(sessionId, sessionToken),
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddHours(this._sessionSettings.ExpiryHours)
            }
        );

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register(
        [FromBody] RegisterRequest request)
    {
        var command = this._mapper.Map<RegisterCommand>(request);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserResponse>>(
            userResult => this.Created(
                string.Empty,
                this._mapper.Map<UserResponse>(userResult)
            ),
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(
        [FromBody] LoginRequest request)
    {
        var command = this._mapper.Map<LoginCommand>(request);
        var result = await this._mediator.Send(command);

        return result.Match<ActionResult<UserResponse>>(
            authResult =>
            {
                this.SetSessionCookie(authResult.SessionId, authResult.SessionToken);
                return this.Ok(this._mapper.Map<UserResponse>(authResult.UserResult));
            },
            errors => errors.ToProblem(this)
        );
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var command = new LogoutCommand();
        var result = await this._mediator.Send(command);

        return result.Match<IActionResult>(
            unit => this.NoContent(),
            errors => errors.ToProblem(this)
        );
    }
}
