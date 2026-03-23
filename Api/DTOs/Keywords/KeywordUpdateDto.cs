using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Keywords
{
    public class KeywordUpdateDto
    {
        [StringLength(500, ErrorMessage = "Value must not exceed 500 characters")]
        public string? Value { get; set; }
    }
}
