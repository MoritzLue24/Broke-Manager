namespace Api.Models
{
    public class Keyword
    {
        public int Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public KeywordType KeywordType { get; set; }

        // Link to Category, one Category, many Keywords
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}