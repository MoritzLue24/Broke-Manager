using Application.Common.Interfaces.Persistence;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await this._dbContext.Users
            .Where(u => u.Email.Value == email)
            .SingleOrDefaultAsync(ct);

    public async Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
        => await this._dbContext.Users.AnyAsync(u => u.Email.Value == email, ct);

    public void Add(User user)
        => this._dbContext.Users.Add(user);
}
