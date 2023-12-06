namespace BlogApi.Models.Entities;

public class Like
{
    public Guid Id { get; set; }
    
    public Guid AuthorId { get; set; }
    public User Author { get; set; }
    
    public Guid PostId { get; set; }
    public Post Post { get; set; }

}