namespace Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TitleKeywords { get; set; } = string.Empty;
        public string CounterPartyKeywords { get; set; } = string.Empty;
        public string Interval { get; set; } = "Once";      // "Once", "Weekly", "Monthly", "Quarterly", "Yearly"
        // Verknüpfung zum User (Jeder User hat eigene Kategorien)
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}