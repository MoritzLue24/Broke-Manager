using Api.DTOs.Users;

namespace Api.Services.User
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserAsync(int id);
        Task<bool> UpdateUserAsync(int id, UserUpdateDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> UpdateRoleAsync(int id, string newRole);
        Task<bool> ChangePasswordAsync(int id, string oldPassword, string newPassword);

    }
}