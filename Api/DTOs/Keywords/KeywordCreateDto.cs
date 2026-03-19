using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Keywords
{
    public class KeywordCreateDto
    {
        [Required(ErrorMessage = "Value is required")]
        [StringLength(500, ErrorMessage = "Value must not exceed 500 characters")]
        public required string Value { get; set; }

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; } 
        
    }
}