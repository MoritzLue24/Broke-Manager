using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common.Interfaces.Persistence;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        => await this._dbContext.Users.AnyAsync(u => u.Email.Value == email, ct);

    public void Add(User user)
        => this._dbContext.Users.Add(user);
}