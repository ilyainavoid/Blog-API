using System.ComponentModel.DataAnnotations;
using BlogApi.Models.Enums;

namespace BlogApi.Models.DTO;

public class UserRegisterModel
{
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    [Required]
    [EmailAddress]
    [MinLength(1)]
    public string Email { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    public Gender Gender { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
}