using Api.Errors;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    // We need this to generate a proper problem response
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, IProblemDetailsService problemDetailsService)
    {
        this._next = next;
        this._env = env;
        this._problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this._next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await this._problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Type = ErrorTypes.Internal,
                    Title = this._env.IsDevelopment()
                        ? ex.Message
                        : "An internal server error occurred.",
                    Detail = this._env.IsDevelopment()
                        ? ex.StackTrace
                        : null
                }
            });
        }
    }
}
