using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Configuration.AddEnvironmentVariables();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddControllers();
    
    builder.Services.AddInfrastructure(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Default connection string not set"));
    builder.Services.AddApplication();
}

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    // app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}
