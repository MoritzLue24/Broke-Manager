using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Api.Data;
using Api.Services.Categories;
using Api.Services.Keywords;
using Api.Middlewares;


var builder = WebApplication.CreateBuilder(args);
string? dbProvider = builder.Configuration["DatabaseProvider"];

if (dbProvider == "SqlServer")
{
    string connectionString = builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new KeyNotFoundException("'SqlServer' Connection string not found");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
}
else if (dbProvider == "Sqlite")
{
    string connectionString = builder.Configuration.GetConnectionString("Sqlite")
        ?? throw new KeyNotFoundException("'Sqlite' Connection string not found");
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


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

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