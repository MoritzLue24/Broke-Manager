namespace Api.DTOs.Keywords
{
    public class KeywordCreateDto
    {
        [Required(ErrorMessage = "Keyword is required")]
        public string Keyword { get; set; }
        
        [Required(ErrorMessage = "IsTitle is required")]
        public bool IsTitle { get; set; }
        
        [Required(ErrorMessage = "IsCounterParty is required")]
        public bool IsCounterParty { get; set; }
    }
}