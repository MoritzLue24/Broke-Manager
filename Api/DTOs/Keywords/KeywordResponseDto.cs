namespace Api.DTOs.Keywords
{
    public class KeywordResponseDto
    {
        public int Id { get; set; }
        public required string Value { get; set; }
        public int CategoryId { get; set; } 
    }
}
