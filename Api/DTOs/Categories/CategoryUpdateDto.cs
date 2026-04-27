using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Categories
{
    public class CategoryUpdateDto
    {
        [StringLength(255, ErrorMessage = "Name must not exceed 255 characters")]
        public string? Name { get; set; }

        [EnumDataType(typeof(IntervalDto), ErrorMessage = "Invalid interval value")]
        public IntervalDto? Interval { get; set; } 
    }
}
