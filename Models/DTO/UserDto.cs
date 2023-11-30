using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using BlogApi.Models.Enums;

namespace BlogApi.Models.DTO;

public class UserDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Email { get; set; }
    
    [Phone]
    [RegularExpression(@"^\+7 \(\d{3}\) \d{3}-\d{2}-\d{2}$")]
    public string? PhoneNumber { get; set; }
}