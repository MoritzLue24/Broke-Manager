public class TransactionCreateDto
{
    public DateTime Date { get; set; } // DateTime? oder was war das anders? 
    public decimal Amount { get; set; } 
    public string CounterParty { get; set; } 
    public string Title { get; set; }
}