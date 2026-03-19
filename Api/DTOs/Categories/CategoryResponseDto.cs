using Api.DTOs.Keywords;
using Api.DTOs.Intervals;

namespace Api.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public List<KeywordResponseDto> Keywords { get; set; } = []; // = new List<KeywordResponseDto>();
        
        public IntervalDto Interval { get; set; }
    }
}