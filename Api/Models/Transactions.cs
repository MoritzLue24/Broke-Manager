namespace Api.Models
{
    public class Transaction
    {
        public int Id { get; set; } // Primärschlüssel
        public DateTime Date { get; set; }
        public decimal Amount { get; set; } // Immer decimal für Geld!
        public string Recipient { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        
        // Die ID-Nummer in der Datenbank, die diese Transaktion fest mit einem User verknüpft (Foreign Key).
        public int UserId { get; set; }
        // Navigation Property: Ermöglicht den Zugriff auf die zugehörigen User-Daten
        public User? User { get; set; }

        // Aanalog zu User oben
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}