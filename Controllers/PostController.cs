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

    [HttpGet]
    public async Task<ActionResult<PostPagedListDto>> GetPosts([FromQuery] QueryParametersPost parameters)
    {
        bool isAuthorized = Request.Headers.ContainsKey("Authorization");

        if (isAuthorized)
        {
            string? userIdString = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
            if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userId))
            {
                try
                {
                    var response = await _postService.GetPosts(userId, parameters);
                    return Ok(response);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
            else
            {
                return StatusCode(500, "Can't parse UserId from token claims");
            }
        }
        else
        {
            try
            {
                var response = await _postService.GetPosts(null, parameters);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
    
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guid>> CreatePost(CreatePostDto model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }
        
        string userIdString = User.Claims.FirstOrDefault(c => c.Type == "id").Value;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out Guid userId))
        {
            try
            {
                var response = await _postService.CreatePersonalPost(userId, model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        else
        {
            return StatusCode(500, "Can't parse UserId from token claims");
        }
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