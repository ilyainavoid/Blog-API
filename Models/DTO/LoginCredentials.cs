using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class LoginCredentials
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Password { get; set; }
}