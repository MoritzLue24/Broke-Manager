using Api.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHostEnvironment _env;
    private readonly IProblemDetailsService _problemDetailsService;

    public ExceptionMiddleware(RequestDelegate next, IHostEnvironment env, IProblemDetailsService problemDetailsService)
    {
        _next = next;
        _env = env;
        _problemDetailsService = problemDetailsService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await _problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails =
                {
                    Type = ErrorTypes.Internal,
                    Title = _env.IsDevelopment()
                        ? ex.Message
                        : "An internal server error occurred.",
                    Detail = _env.IsDevelopment()
                        ? ex.StackTrace
                        : null
                }
            });
        }
    }
}