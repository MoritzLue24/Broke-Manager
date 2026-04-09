using Api.Data;
using Microsoft.EntityFrameworkCore;
using Api.DTOs.Users;
using Api.Models;
using Api.Exceptions;

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
                throw new NotFoundException("User not found");
            }

            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email
            };
        }

        // TODO unique email checkbei update
        public async Task UpdateUserAsync(int id, UserUpdateDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            
             if (user == null)
            {
                throw new NotFoundException("User not found");
            }


            if (!string.IsNullOrEmpty(updateDto.Email))
            {
                user.Email = updateDto.Email;
            }
            
            await _dbContext.SaveChangesAsync();
            
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateRoleAsync(int id, Role newRole)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) 
            {
                throw new NotFoundException("User not found");
            }
            

            if (newRole != Role.Admin && newRole != Role.User) 
            {
                throw new InvalidRoleException("Invalid role") ;
            }

            user.Role = newRole; 
            await _dbContext.SaveChangesAsync();
        }

        public async Task ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) 
            {
                throw new NotFoundException("User not found");
            }

            // Check if the old password is correct
            if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
            {
                throw new IncorrectPasswordException("Incorrect old password");
            }

            // Hash the new password and update the user's password hash
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            await _dbContext.SaveChangesAsync();
        }
    }
}