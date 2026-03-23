using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Users
{
    public class UserUpdateDto
    {
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(255, ErrorMessage = "Email must not exceed 255 characters")]
        public string? Email { get; set; }
    }
}