using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class CreatePostDto
{
    [Required]
    [MinLength(5)]
    public string Title { get; set; }
    
    [Required]
    [MinLength(5)]
    public string Description { get; set; }
    
    [Required]
    public int ReadingTime { get; set; }
    
    public Uri? Image { get; set; }
    
    public Guid? AddressId { get; set; }
    
    [Required]
    [MinLength(1)]
    public List<Guid> Tags { get; set; }
}