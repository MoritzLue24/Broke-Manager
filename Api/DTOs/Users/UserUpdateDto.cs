using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Users
{
    public class UserUpdateDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public  required string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [StringLength(255, ErrorMessage = "Password must not exceed 255 characters")]
        public  required string Password { get; set; }
    }
}