using Domain.Entities;

namespace Application.Common.Interfaces.Persistence;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<bool> EmailExistsAsync(string email, CancellationToken ct = default);

    void Add(User user);
}
