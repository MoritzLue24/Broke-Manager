using System.Diagnostics;
using Api.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Configuration;

public class InvalidModelStateConfiguration : IConfigureOptions<ApiBehaviorOptions>
{
    public void Configure(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = ctx =>
        {
            var problemDetails = new ValidationProblemDetails(ctx.ModelState)
            {
                Type = ErrorTypes.Validation,
                Status = StatusCodes.Status400BadRequest,
                Title = "One or more validation errors occurred."
            };
            problemDetails.Extensions["traceId"] = Activity.Current?.Id
                ?? ctx.HttpContext.TraceIdentifier;
            return new BadRequestObjectResult(problemDetails);
        };
    }
}
