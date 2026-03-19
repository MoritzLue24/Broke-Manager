using System.ComponentModel.DataAnnotations;

public class TransactionCreateDto
{
    [Required(ErrorMessage = "Date is required")]
    public DateOnly Date { get; set; } 
    
    [Required(ErrorMessage = "Amount is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
    public decimal Amount { get; set; } 
    
    [Required(ErrorMessage = "CounterParty is required")]
    public string CounterParty { get; set; } 
    
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "CategoryId is required")]
    public int CategoryId { get; set; }
}