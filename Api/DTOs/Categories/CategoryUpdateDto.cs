using System.ComponentModel.DataAnnotations;

public class CategoryUpdateDto
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "TitleKeywords is required")]
    public string[] TitleKeywords { get; set; }
    
    [Required(ErrorMessage = "CounterPartyKeywords is required")]
    public string[] CounterPartyKeywords { get; set; }
    
    [Required(ErrorMessage = "Interval is required")]
    public string Interval { get; set; }
}