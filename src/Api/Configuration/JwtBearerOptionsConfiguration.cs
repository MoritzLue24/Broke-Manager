using Api.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Api.Configuration;

// Configures the http side of JwtBearerOptions, because
// we dont want to change http things like responses inside the Infrastructure layer
public class JwtBearerOptionsConfiguration : IConfigureNamedOptions<JwtBearerOptions>
{
    public void Configure(string? name, JwtBearerOptions options)
    {
        options.Events ??= new JwtBearerEvents();
        options.Events.OnChallenge = async ctx =>
        {
            ctx.HandleResponse();
            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
            ctx.Response.ContentType = "application/problem+json";
            await ctx.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Type = ErrorTypes.Unauthorized,
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized"
            });
        };
    }

    public void Configure(JwtBearerOptions options)
        => this.Configure(string.Empty, options);
}
