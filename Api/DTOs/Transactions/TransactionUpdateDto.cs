using System.ComponentModel.DataAnnotations;

namespace Api.DTOs.Transactions
{
    public class TransactionUpdateDto
    {
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format")]
        public DateOnly Date { get; set; } 
        
        
        [Required(ErrorMessage = "Amount is required")]
        [RegularExpression(@"^\d{1,12}(\.\d{1,2})?$",
        ErrorMessage = "Max. 12 Stellen vor und 2 nach dem Komma")] // Allows up to 12 digits before the decimal point and 2 digits after
        public required decimal Amount { get; set; }
        
        [Required(ErrorMessage = "CounterParty is required")]
        [StringLength(255, ErrorMessage = "CounterParty must not exceed 255 characters")]
        public required string CounterParty { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [StringLength(500, ErrorMessage = "Title must not exceed 500 characters")]
        public required string Title { get; set; }
        
        [Range(0, int.MaxValue, ErrorMessage = "CategoryId must be a non-negative integer")]
        public int? CategoryId { get; set; }
    }
}