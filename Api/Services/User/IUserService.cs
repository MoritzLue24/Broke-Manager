using Api.DTOs.Users;
using Api.Models;


namespace Api.Services.User
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserAsync(int id);
        Task UpdateUserAsync(int id, UserUpdateDto dto);
        Task DeleteUserAsync(int id);
        Task UpdateRoleAsync(int id, Role newRole);
        Task ChangePasswordAsync(int id, string oldPassword, string newPassword);

    }
}