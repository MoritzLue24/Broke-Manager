using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Api.Data;
using Api.Services.Categories;
using Api.Services.Keywords;
using Api.Middlewares;
using Api.Exceptions;
using Api.Services.Auth;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;


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
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Broke-Manager-Api", Version = "v1" });

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT Token",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IKeywordService, KeywordService>();

string string_key = builder.Configuration["Jwt:Key"] 
    ?? throw new MissingConfigurationException("Jwt-Key not found in appsettings.json");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(string_key))
    };
});
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

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