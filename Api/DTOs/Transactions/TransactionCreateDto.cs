using System.ComponentModel.DataAnnotations;


namespace Api.DTOs.Transactions
{
    public class TransactionCreateDto
    {
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateOnly Date { get; set; } 
        
        [Required(ErrorMessage = "Amount is required")]
        [RegularExpression(@"^\d{1,12}(\.\d{1,2})?$",
        ErrorMessage = "Max. 12 Stellen vor und 2 nach dem Komma")] // Allows up to 12 digits before the decimal point and 2 digits after
        public decimal Amount { get; set; } 
        
        [StringLength(255, ErrorMessage = "CounterParty must not exceed 255 characters")]
        public string CounterParty { get; set; } = string.Empty; 
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(500, ErrorMessage = "Title must not exceed 500 characters")]
        public required string Title { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "CategoryId must be a non-negative integer")]
        /*
        Das [Range]-Attribut ist clever genug: Wenn der Wert null ist (also nicht mitgeschickt wurde), 
        ignoriert es die Prüfung. Wenn jedoch ein Wert geschickt wird, muss dieser weiterhin zwischen 0 und dem Maximum liegen.
        */
        public  int? CategoryId { get; set; }
    }
}
