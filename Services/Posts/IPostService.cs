using BlogApi.Models.DTO;

namespace BlogApi.Services.Posts;

public interface IPostService
{
    Task<PostPagedListDto> GetPosts(Guid? userId, QueryParametersPost parametersPost);
    Task<Guid> CreatePersonalPost(Guid userId, CreatePostDto model);
    Task<PostFullDto> GetPostInfo(Guid userId, Guid postId);
    Task AddLikeToPost(Guid userId, Guid postId);
    Task DeleteLikeFromPost(Guid userId, Guid postId);
}