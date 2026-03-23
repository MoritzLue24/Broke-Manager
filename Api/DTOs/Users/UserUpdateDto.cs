using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Users
{
    public class UserUpdateDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Required(ErrorMessage = "Email is required")]
        [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public  required string Email { get; set; }
    }
}