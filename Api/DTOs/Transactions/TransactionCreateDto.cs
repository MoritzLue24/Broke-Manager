public class TransactionCreateDto
{
    public DateOnly Date { get; set; } // DateTime? oder was war das anders? 
    public decimal Amount { get; set; } 
    public string CounterParty { get; set; } 
    public string Title { get; set; }
    public int CategoryId { get; set; }
}