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
             
            return await _userService.GetUserAsync(id); //ist gelb aber du meintest ich soll nicht Exceptions im Controller cathcen sondern im Service.
        }
        
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<ActionResult> UpdateUserById([FromRoute] int id, [FromBody] UserUpdateDto updateDto)
        {
            
            await _userService.UpdateUserAsync(id, updateDto);
            
            

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUserById([FromRoute] int id)
        {
            await _userService.DeleteUserAsync(id);
            
            return NoContent();
            
        }

        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeUserRole([FromRoute] int id, [FromBody] ChangeUserRoleDto dto)
        {

            await _userService.UpdateRoleAsync(id, dto);
        
            return Ok(new { message = $"Role updated successfully to {dto.NewRole}" });
        }



        // --------------------------------------------------
        // USER   ZONE
        // --------------------------------------------------

        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetMe()
        {
            int id = GetCurrentUserId();     
            var user = await _userService.GetUserAsync(id);
            return user; //ist gelb aber du meintest ich soll nicht Exceptions im Controller cathcen sondern im Service.
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateMe([FromBody] UserUpdateDto updateDto)
        {
            int CurrentUserId = GetCurrentUserId();
            await _userService.UpdateUserAsync(CurrentUserId, updateDto);
            
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<ActionResult> DeleteMe()
        {
            int CurrentUserId = GetCurrentUserId();
            await _userService.DeleteUserAsync(CurrentUserId);
            
            return NoContent();
        }

        [HttpPut("me/change")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] ChangePasswordDto dto)
        {
            
            
            int userId = GetCurrentUserId();
            
            await _userService.ChangePasswordAsync(userId, dto);

            return Ok(new { message = "Password updated successfully" });
        }

    }
}
