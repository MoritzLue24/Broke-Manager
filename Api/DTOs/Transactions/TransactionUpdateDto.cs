public class TransactionUpdateDto
{
    public DateTime Date { get; set; } //Datetime?
    public decimal Amount { get; set; }
    public string CounterParty { get; set; }
    public string Title { get; set; }
}