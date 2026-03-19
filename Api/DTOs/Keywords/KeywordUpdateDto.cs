using System.ComponentModel.DataAnnotations;
using Api.Models;

namespace Api.DTOs.Keywords
{
    public class KeywordUpdateDto
    {
        [Required(ErrorMessage = "Value is required")]
        [StringLength(500, ErrorMessage = "Value must not exceed 500 characters")]
        public required string Value { get; set; }
        
        
    }

}