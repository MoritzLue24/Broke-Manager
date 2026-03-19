using Api.Models;

namespace Api.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string[] TitleKeywords { get; set; }
        public string[] CounterPartyKeywords { get; set; }
        public Interval Interval { get; set; }
    }
}