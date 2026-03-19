using System.ComponentModel.DataAnnotations;
using Api.DTOs;
using Api.DTOs.Keywords;


namespace Api.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name must not exceed 255 characters")]
        public required string Name { get; set; }

        public IntervalDto Interval { get; set; } = IntervalDto.Once; 
    }
}
