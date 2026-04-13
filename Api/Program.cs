using Microsoft.EntityFrameworkCore;
using Api.Data;
using DotNetEnv;
using Api.Services.User;


Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Api.Services.User.IUserService, Api.Services.User.UserService>();
builder.Services.AddScoped<Api.Services.Transaction.ITransactionService, Api.Services.Transaction.TransactionService>();
builder.Services.AddScoped<Api.Services.Token.ITokenService, Api.Services.Token.TokenService>();
builder.Services.AddScoped<Api.Services.Auth.IAuthService, Api.Services.Auth.AuthService>();

var app = builder.Build();

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