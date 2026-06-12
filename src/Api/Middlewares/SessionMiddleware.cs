using Application.Features.Auth.Contracts;
using Application.Features.Auth.Interfaces;

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
        ISessionCookieService sessionService,
        ISessionSettings sessionSettings)
    {
        var cookieValue = context.Request.Cookies[sessionSettings.CookieName];

        if (!string.IsNullOrWhiteSpace(cookieValue))
        {
            SessionResult? sessionResult;
            if ((sessionResult = await sessionService.ValidateCookieAsync(cookieValue)) is not null)
            {
                // TODO: not hardcoded?
                context.Items["sessionId"] = sessionResult.Id;
                context.Items["userId"] = sessionResult.UserId;
                context.Items["roles"] = sessionResult.Roles;
            }
            else
                context.Response.Cookies.Delete(sessionSettings.CookieName);
        }

        await this._next(context);
    }
}
