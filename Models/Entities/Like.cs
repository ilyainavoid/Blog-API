namespace BlogApi.Models.Entities;

public class Like
{
    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }

    public Guid Author { get; set; }
}