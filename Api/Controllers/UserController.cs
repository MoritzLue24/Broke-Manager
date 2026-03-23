using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.DTOs.Users;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // TODO: Als admin
        [HttpGet]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
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

        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetUser()
        {
            int id = 0;     // TODO
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email
            };
            return userDto;
        }

        [HttpPut("me")]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] UserUpdateDto updateDto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.Email = updateDto.Email;

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("me")]
        public async Task<ActionResult> DeleteUser()
        {
            int id = 0;     // TODO
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // TODO: Als Admin
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser([FromRoute] int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
