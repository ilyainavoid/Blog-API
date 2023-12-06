using BlogApi.Models.DTO;

namespace BlogApi.Services.Posts;

public class PostService : IPostService
{
    public Task<PostPagedListDto> GetPosts(Guid userId, QueryParametersPost parametersPost)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreatePersonalPost(Guid userId, CreatePostDto model)
    {
        throw new NotImplementedException();
    }

    public Task<PostFullDto> GetPostInfo(Guid userId, Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task AddLikeToPost(Guid userId, Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLikeFromPost(Guid userId, Guid postId)
    {
        throw new NotImplementedException();
    }
}