using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class TokenResponse
{
    [Required]
    [MinLength(1)]
    public string Token { get; set; }
}