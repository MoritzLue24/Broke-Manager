using System.ComponentModel.DataAnnotations;
using Api.Models;

namespace Api.DTOs.Users
{
    public class ChangeUserRoleDto
    {
        [Required(ErrorMessage = "Role is required")]
        [EnumDataType(typeof(Role), ErrorMessage = "Invalid role value")]
        public required Role NewRole { get; set; }
    }
}