using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.Entities;

public class Community(List<User> administrators, List<Post> posts)
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreateTime { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Name { get; set; }
    
    public string? Description { get; set; }

    [Required] 
    public bool IsClosed { get; set; } = false;

    [Required] 
    public int SubscribersCount { get; set; } = 0;
    
    public List<User> Administrators = administrators;
    public List<Post> Posts = posts;

    public ICollection<User> Subscribers { get; set; }
}