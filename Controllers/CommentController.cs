using System.Security.Claims;
using BlogApi.Exceptions;
using BlogApi.Models.DTO;
using BlogApi.Services.Comments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("comment/{id}/tree")]
    public async Task<ActionResult> GetTree(Guid id)
    {
        string? userId = null;
        var idClaim = HttpContext.Items["userId"];
        if (idClaim != null)
        {
            userId = idClaim.ToString();
        }

        try
        {
            var response = await _commentService.GetTree(id, userId);
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
    [HttpPost("post/{id}/comment")]
    public async Task<ActionResult> AddComment(Guid id, [FromBody] CreateCommentDto model)
    {
        string? userId = null;
        var idClaim = HttpContext.Items["userId"];
        if (idClaim != null)
        {
            userId = idClaim.ToString();
        }
        else
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "User is unauthorized"
            };
            return StatusCode(401, response);
        }
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _commentService.AddComment(id, userId, model);
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
    [HttpPut("comment/{id}")]
    public async Task<ActionResult> EditComment(Guid id, [FromBody] UpdateCommentDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _commentService.EditComment(id, userId, model);
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
    [HttpDelete("comment/{id}")]
    public async Task<ActionResult> DeleteComment(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        try
        {
            await _commentService.DeleteComment(id, userId);
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