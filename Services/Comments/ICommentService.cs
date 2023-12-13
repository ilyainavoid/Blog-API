using BlogApi.Models.DTO;

namespace BlogApi.Services.Comments;

public interface ICommentService
{
    Task<List<CommentDto>> GetTree(Guid id, string userId);
    Task AddComment(Guid id, string userId, CreateCommentDto commentModel);
    Task EditComment(Guid id, string userId, UpdateCommentDto commentModel);
    Task DeleteComment(Guid id, string userId);
}