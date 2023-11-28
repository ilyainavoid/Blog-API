namespace BlogApi.Models.Entities;

public class Like
{
    public Guid Id { get; set; }
    public User Author { get; set; }

    public Post Post { get; set; }

}