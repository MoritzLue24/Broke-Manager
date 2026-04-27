using System.Text.Json;
using Api.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        public static Task HandleException(HttpContext context, Exception e)
        {
            var statusCode = e switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                AlreadyExistsException => StatusCodes.Status409Conflict,
                MissingConfigurationException => StatusCodes.Status500InternalServerError,

                DbUpdateConcurrencyException => StatusCodes.Status499ClientClosedRequest,
                DbUpdateException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };
            var response = new
            {
                message = e.Message,
                status = statusCode
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}