using Application.Features.Auth.Contracts;
using Application.Features.Auth.Interfaces;
using Application.Features.Auth.Services;

namespace Api.Middlewares;

public class SessionMiddleware
{
    private readonly RequestDelegate _next;

    public SessionMiddleware(RequestDelegate next)
    {
        this._next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ISessionCookieParser sessionCookieParser,
        SessionService sessionService,
        ISessionSettings sessionSettings)
    {
        var cookieValue = context.Request.Cookies[sessionSettings.CookieName];

        if (sessionCookieParser.ParseCookie(cookieValue ?? string.Empty)
            is (Guid sessionId, string sessionToken))
        {
            if (await sessionService.Validate(sessionId, sessionToken)
                is SessionResult sessionResult)
            {
                // TODO: not hardcoded?
                context.Items["sessionId"] = sessionResult.Id;
                context.Items["userId"] = sessionResult.UserId;
                context.Items["roles"] = sessionResult.Roles;

                _ = sessionService.Visit(sessionResult.Id, sessionResult.LastSeen);
            }
            else
                context.Response.Cookies.Delete(sessionSettings.CookieName);
        }

        await this._next(context);
    }
}
