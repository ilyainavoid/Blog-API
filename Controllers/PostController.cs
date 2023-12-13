using BlogApi.Exceptions;
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
        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.ToString(), out var id))
            {
                userId = id;
            }
        }
        
        try
        {
            var response = await _postService.GetPosts(userId, parameters);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(404, response);
        }
        catch (BadRequestException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return BadRequest(response);
        }
        catch (ForbiddenException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(403, response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, ex.Message);
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

        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null)
        {

            if (Guid.TryParse(idClaim.ToString(), out var id))
            {
                userId = id;
            }
        }


        try
        {
            var response = await _postService.CreatePersonalPost(userId, model);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(404, response);
        }
        catch (BadRequestException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return BadRequest(response);
        }
        catch (ForbiddenException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(403, response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<PostFullDto>> GetPostInfo(Guid id)
    {

        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null)
        {

            if (Guid.TryParse(idClaim.ToString(), out var parsedId))
            {
                userId = parsedId;
            }
        }

        try
        {
            var response = await _postService.GetPostInfo(userId, id);
            return Ok(response);
        }
        catch (NotFoundException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(404, response);
        }
        catch (BadRequestException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return BadRequest(response);
        }
        catch (ForbiddenException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(403, response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpPost("{postId}/like")]
    public async Task<ActionResult<Response>> AddLike(Guid postId)
    {
        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null)
        {

            if (Guid.TryParse(idClaim.ToString(), out var id))
            {
                userId = id;
            }
        }

        try
        {
            await _postService.AddLikeToPost(userId, postId);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(404, response);
        }
        catch (BadRequestException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return BadRequest(response);
        }
        catch (ForbiddenException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(403, response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, ex.Message);
        }
    }
    
    [Authorize]
    [HttpDelete("{postId}/like")]
    public async Task<ActionResult<Response>> DeleteLike(Guid postId)
    {
        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null)
        {

            if (Guid.TryParse(idClaim.ToString(), out var id))
            {
                userId = id;
            }
        }

        try
        {
            await _postService.DeleteLikeFromPost(userId, postId);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(404, response);
        }
        catch (BadRequestException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return BadRequest(response);
        }
        catch (ForbiddenException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(403, response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, ex.Message);
        }
    }
}