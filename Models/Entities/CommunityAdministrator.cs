using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.Entities;

public class CommunityAdministrator
{
    [Required]
    public Guid UserId { get; set; }
    public User User { get; set; }

    [Required]
    public Guid CommunityId { get; set; }
    public Community Community { get; set; }
}