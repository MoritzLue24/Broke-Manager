namespace Api.DTOs.Auth
{
    /// <summary>
    /// All fields are stored in a JWT
    /// </summary>
    public class UserClaimsDto
    {
        public int UserId { get; set; }
        public string Role { get; set; } = "User";
    }
}