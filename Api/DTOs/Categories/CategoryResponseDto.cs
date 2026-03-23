using Api.DTOs.Keywords;


namespace Api.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<KeywordResponseDto> Keywords { get; set; } = []; // = new List<KeywordResponseDto>();
        public IntervalDto Interval { get; set; }
        public bool IsDefault { get; set; }
    }
}
