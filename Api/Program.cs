using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Api.Data;
using Api.Services.Categories;
using Api.Services.Keywords;
using Api.Middlewares;
using Api.Exceptions;


var builder = WebApplication.CreateBuilder(args);
string? dbProvider = builder.Configuration["DatabaseProvider"];

if (dbProvider == "SqlServer")
{
    string connectionString = builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new MissingConfigurationException("'SqlServer' Connection string not found");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
}
else if (dbProvider == "Sqlite")
{
    string connectionString = builder.Configuration.GetConnectionString("Sqlite")
        ?? throw new MissingConfigurationException("'Sqlite' Connection string not found");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
}
else
{
    throw new NotImplementedException("Invalid Database provider in appsettings");
}

builder.Services.AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
    );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IKeywordService, KeywordService>();

// TODO: Restrict CORS in production
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // frontend URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}
app.MapControllers();
app.Run();