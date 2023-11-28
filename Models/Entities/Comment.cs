namespace BlogApi.Models.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string Content { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeleteDate { get; set; }
    public Guid AuthorId { get; set; }
    public string AuthorName { get; set; }
    public int SubComments { get; set; }
    
    public Guid? ParentCommentId { get; set; }
    
    public List<Comment> ChildComments { get; set; }
}