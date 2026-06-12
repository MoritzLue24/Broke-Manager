using Application.Common.Interfaces.Security;

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
        if (string.IsNullOrWhiteSpace(cookieValue))
        {
            await this._next(context);
            return;
        }

        var validatedSession = await sessionService.ValidateCookieAsync(cookieValue);
        if (validatedSession is null)
        {
            await this._next(context);
            return;
        }

        // TODO: not hardcoded?
        context.Items["sessionId"] = validatedSession.Id;
        context.Items["userId"] = validatedSession.UserId;
        context.Items["roles"] = validatedSession.Roles;
        await this._next(context);
    }
}
