using BlogApi.Models.Enums;

namespace BlogApi.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Password { get; set; }
    public string FullName { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    
    public List<Comment> Comments { get; set; }
}