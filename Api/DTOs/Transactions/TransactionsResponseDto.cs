namespace Api.DTOs.Transactions
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public string CounterParty { get; set; } = string.Empty;
        public required string Title { get; set; }
        public int CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }
}
