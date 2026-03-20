using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Api.Data;
using Api.Models;
using Api.DTOs.Users;


namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public UsersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _dbContext.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    Email = u.Email
                })
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
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
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userDto)
        {
            var newUser = new User
            {
                Email = userDto.Email,
                Password = userDto.Password
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            var createdUserDto = new UserResponseDto
            {
                Id = newUser.Id,
                Email = newUser.Email
            };


            return CreatedAtAction(nameof(GetUser), new { id = newUser.Id }, createdUserDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDto userDto)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(userDto.Email))
            {
                user.Email = userDto.Email;
            }
            if (!string.IsNullOrEmpty(userDto.Password))
            {
                user.Password = userDto.Password;
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
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
