namespace Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Interval Interval { get; set; } = Interval.Once;
        // Verknüpfung zum User (Jeder User hat eigene Kategorien)
        public int UserId { get; set; }
        public bool IsDefault { get; set; }
        public User User { get; set; } = null!;
        public List<Keyword> Keywords { get; set; } = [];
    }
}