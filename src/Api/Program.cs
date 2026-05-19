using Infrastructure;
using Application;
using Api;
using Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddInfrastructure(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Default connection string not set"));
    builder.Services.AddApplication();
    builder.Services.AddApi();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();
    app.UseMiddleware<ExceptionMiddleware>();
    app.MapControllers();
    app.Run();
}

