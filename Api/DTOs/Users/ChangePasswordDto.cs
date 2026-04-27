using System.ComponentModel.DataAnnotations;

// TODO nicht erlauben wenn altes und neues passwort gelich sind
namespace Api.DTOs.Users
{
    public class ChangePasswordDto
    {
        

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [StringLength(255, ErrorMessage = "Password must not exceed 255 characters")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "New Password must be at least 8 characters long")]
        [StringLength(255, ErrorMessage = "New Password must not exceed 255 characters")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [MinLength(8, ErrorMessage = "Confirmed new Password must be at least 8 characters long")]
        [StringLength(255, ErrorMessage = "New Password must not exceed 255 characters")]
        public required string ConfirmNewPassword { get; set; }
    }
}