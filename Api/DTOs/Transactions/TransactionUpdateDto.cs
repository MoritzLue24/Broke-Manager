using System.ComponentModel.DataAnnotations;

public class TransactionUpdateDto
{
    [Required(ErrorMessage = "Date is required")]
    public DateOnly Date { get; set; } 
    
    
    [Required(ErrorMessage = "Amount is required")]
    public decimal Amount { get; set; }
    
    [Required(ErrorMessage = "CounterParty is required")]

    public string CounterParty { get; set; }
    
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "CategoryId is required")]
    public int CategoryId { get; set; }
}