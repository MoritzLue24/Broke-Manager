namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty; 

        // Ein User hat viele Transaktionen und viele Kategorien
        public List<Transaction> Transactions { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}