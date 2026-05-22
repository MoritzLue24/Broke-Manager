using Api;
using Api.Middlewares;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddInfrastructure(builder.Configuration);
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

