using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models.DTO;

public class CreateCommentDto
{
   [Required]
   [MinLength(1)]
   public string Content { get; set; }
   
   public Guid? ParentId { get; set; }
}