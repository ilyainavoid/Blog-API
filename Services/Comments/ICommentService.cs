using BlogApi.Models.DTO;

namespace BlogApi.Services.Comments;

public interface ICommentService
{
    Task<List<CommentDto>> GetTree(Guid id, Guid userId);
    Task AddComment(Guid id, Guid userId, CreateCommentDto commentModel);
    Task EditComment(Guid id, Guid userId, UpdateCommentDto commentModel);
    Task DeleteComment(Guid id, Guid userId);
}