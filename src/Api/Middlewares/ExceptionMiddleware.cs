using Api.Errors;
using Microsoft.AspNetCore.Mvc;

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
            ProblemDetails problemDetails = new()
            {
                Type = ErrorTypes.Internal,
                Title = "An internal server error occurred.",
                Detail = this._env.IsDevelopment()
                    ? ex.Message
                    : null
            };

            if (this._env.IsDevelopment())
            {
                problemDetails.Extensions.Add("exception", ex.GetType().ToString());
                problemDetails.Extensions.Add("stackTrace", ex.StackTrace);
            }

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await this._problemDetailsService.WriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = problemDetails
            });
        }
    }
}
