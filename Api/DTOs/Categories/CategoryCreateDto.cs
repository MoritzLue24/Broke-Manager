using System.ComponentModel.DataAnnotations;
using Api.Models;
using Api.DTOs.Keywords;


namespace Api.DTOs.Categories
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(255, ErrorMessage = "Name must not exceed 255 characters")]
        public required string Name { get; set; }

        public List<KeywordCreateDto> TitleKeywords { get; set; } = []; // = new List<KeywordCreateDto>();
        
        public Interval Interval { get; set; } = Interval.Once; 

    }
}