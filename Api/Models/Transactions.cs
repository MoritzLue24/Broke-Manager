namespace Api.Models
{
    public class Transaction
    {
        public int Id { get; set; }     // pk
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }     // Immer decimal für Geld!
        public string CounterParty { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int UserId { get; set; }     // fk
        public User? User { get; set; }     // Navigation Property: Ermöglicht den Zugriff auf die zugehörigen User-Daten
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}