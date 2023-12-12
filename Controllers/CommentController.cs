using System.Security.Claims;
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
        try
        {
            var response = await _commentService.GetTree(id);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(500, response);
        }
    }

    [Authorize]
    [HttpPost("post/{id}/comment")]
    public async Task<ActionResult> AddComment(Guid id, [FromBody] CreateCommentDto model)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            await _commentService.AddComment(id, userId, model);
            return Ok();
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(500, response);
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
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(500, response);
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
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(500, response);
        }
    }
}