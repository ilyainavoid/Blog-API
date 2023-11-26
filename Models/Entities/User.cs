using System.ComponentModel.DataAnnotations;
using BlogApi.Models.Enums;

namespace BlogApi.Models.Entities;

public class User()
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public DateTime CreateTime { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    [Required]
    [MinLength(1)]
    public string FullName { get; set; }
    
    public DateTime? BirthDate { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    [MinLength(1)]
    public string Email { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public List<Comment> Comments { get; set; }
    public List<Like> Likes { get; set; }
    public List<Post> Posts {get; set; }

    public List<CommunitySubscriber> Subscriptions { get; set; }
    public List<CommunityAdministrator> ManagedCommunities { get; set; }
}