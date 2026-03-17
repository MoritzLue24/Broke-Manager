namespace Api.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string TitleKeywords { get; set; } = string.Empty;
        public string SenderKeywords { get; set; } = string.Empty;
        
        // Intervall (z.B. "Monthly", "Once")
        public string Interval { get; set; } = "Once";

        // Verknüpfung zum User (Jeder User hat eigene Kategorien)
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}