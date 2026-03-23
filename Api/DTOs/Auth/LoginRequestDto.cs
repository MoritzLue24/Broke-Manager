using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [StringLength(255, ErrorMessage = "Password must not exceed 255 characters")]
        public required string Password { get; set; }
    }
}