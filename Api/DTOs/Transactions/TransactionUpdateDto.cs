public class TransactionUpdateDto
{
    public DateOnly Date { get; set; } //Datetime?
    public decimal Amount { get; set; }
    public string CounterParty { get; set; }
    public string Title { get; set; }
    public int CategoryId { get; set; }
}