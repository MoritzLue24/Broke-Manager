using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Api.Data;
using Api.DTOs.Users;
using Api.Services.User;
using System.Security.Claims;
using Api.Models;



namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        // --- HILFSMETHODE ---
        // HOLT USER-ID AUS DEM JWT TOKEN
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int.TryParse(userIdClaim, out int userId);
            return userId;
        }

        // --------------------------------------------------
        // ADMIN ZONE
        // --------------------------------------------------
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return users;

        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseDto>> GetUserById([FromRoute] int id)
        {
            var user = await _userService.GetUserAsync(id);
            
            if (user == null) 
            {
            return NotFound(new { message = "User not found" });
            }
            
            return user;
        }
        
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult> UpdateUserById([FromRoute] int id, [FromBody] UserUpdateDto updateDto)
        {
            
            var success = await _userService.UpdateUserAsync(id, updateDto);
            
            if (!success) 
            {
                return NotFound(new { message = "User not found" });
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserById([FromRoute] int id)
        {
            var succes = await _userService.DeleteUserAsync(id);
            if (!succes)
            {
                return NotFound(new { Message = "User not found" });
            }
            return NoContent();
            
        }

        [HttpPatch("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] int id, [FromBody] Role newRole)
        {
            
            if (newRole != Role.Admin && newRole != Role.User)
            {
                return BadRequest(new { message = "Allowable Roles: Admin or User" });
            }

            var success = await _userService.UpdateRoleAsync(id, newRole);
            if (!success) 
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(new { message = $"Role updated successfully to {newRole}" });
        }



        // --------------------------------------------------
        // USER   ZONE
        // --------------------------------------------------

        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetMe()
        {
            int id = GetCurrentUserId();     
            var user = await _userService.GetUserAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }
            return user;
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateMe([FromBody] UserUpdateDto updateDto)
        {
            int CurrentUserId = GetCurrentUserId();
            var success = await _userService.UpdateUserAsync(CurrentUserId, updateDto);
            if (!success)
            {
                return NotFound(new { Message = "User not found" });
            }
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<ActionResult> DeleteMe()
        {
            int CurrentUserId = GetCurrentUserId();
            var success = await _userService.DeleteUserAsync(CurrentUserId);
            if (!success)
            {
                return NotFound(new { Message = "User not found" });
            }
            return NoContent();
        }

        [HttpPut("me/password")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] ChangePasswordDto dto)
        {
            
            if (dto.NewPassword != dto.ConfirmNewPassword) 
                return BadRequest(new { message = "New passwords do not match" });

            int userId = GetCurrentUserId();

            
            var success = await _userService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            
            if (!success) 
            {
            return BadRequest(new { message = "Invalid current password" });
            }

            return Ok(new { message = "Password updated successfully" });
        }

    }
}
