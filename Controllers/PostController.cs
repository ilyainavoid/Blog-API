using BlogApi.Models.DTO;
using BlogApi.Services.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/post")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<PostPagedListDto>> GetPosts([FromQuery] QueryParametersPost parameters)
    {
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePost(CreatePostDto model)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult<PostFullDto>> GetPostInfo(Guid id)
    {
        throw new NotImplementedException();
    }

    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<ActionResult<Response>> AddLike(Guid postId)
    {
        throw new NotImplementedException();
    }
    
    [Authorize]
    [HttpDelete("{postId}/like")]
    public async Task<ActionResult<Response>> DeleteLike(Guid postId)
    {
        throw new NotImplementedException();
    }
}