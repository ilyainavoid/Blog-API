using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class UpdateCommentDto
{
    [Required]
    [MinLength(1)]
    public string Content { get; set; }
}