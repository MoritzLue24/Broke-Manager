using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.DTOs.Users;
using Api.Models;

namespace Api.Services.User
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _dbContext;
        public UserService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserResponseDto>> GetAllUsersAsync()
        {
             var users = await _dbContext.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserResponseDto?> GetUserAsync(int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        // TODO unique email checkbei update
        public async Task<bool> UpdateUserAsync(int id, UserUpdateDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            
            if (user == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }
            
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoleAsync(int id, string newRole)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) 
            {
                return false;
            }

            if (newRole != "Admin" && newRole != "User") 
            {
                return false;
            }

            user.Role = newRole; 
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) 
            {
                return false;
            }

            // Check if the old password is correct
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                return false; // Old password is incorrect
            }

            // Hash the new password and update the user's password hash
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}