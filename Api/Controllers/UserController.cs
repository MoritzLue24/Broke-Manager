using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Api.Data;
using Api.DTOs.Users;
using Api.Services.User;
using System.Security.Claims;



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
            var succes = await _userService.DeleteAsync(id);
            if (!succes)
            {
                return NotFound(new { Message = "User not found" });
            }
            return NoContent();
            
        }

        // TODO  1. GET BY ID (ADMIN) 2.ROLE CHANGE BY ADMIN 3. PASSWORT ÄNDERN (ME) 4. PASSWORT ÄNDERN (ADMIN)


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
        public async Task<ActionResult> UpdateMe(int id, [FromBody] UserUpdateDto updateDto)
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
            var success = await _userService.DeleteAsync(CurrentUserId);
            if (!success)
            {
                return NotFound(new { Message = "User not found" });
            }
            return NoContent();
        }

    }
}
