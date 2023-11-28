using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.Entities;

public class Community()
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


    public List<CommunityAdministrator> Administrators { get; set; }
    public List<Post> Posts { get; set; }
    public List<CommunitySubscriber> Subscribers { get; set; }
}