using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        [StringLength(255, ErrorMessage = "Name must not exceed 255 characters")]
        public string? Name { get; set; }

        public IntervalDto Interval { get; set; } = IntervalDto.Once; 
    }
}
