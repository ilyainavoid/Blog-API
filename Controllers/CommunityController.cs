using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Services.Communities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/community")]
public class CommunityController : ControllerBase
{
    private readonly ICommunityService _communityService;

    public CommunityController(ICommunityService communityService)
    {
        _communityService = communityService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CommunityDto>>> GetAllCommunities()
    {
        try
        {
            var response = await _communityService.GetAllCommunities();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Smth went wrong");
        }
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<List<CommunityUserDto>>> GetMyCommunities()
    {
        var idClaim = HttpContext.User.FindFirst("id");
        var response = new List<CommunityUserDto>();
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.Value, out var userId))
            {
                try
                {
                    response = await _communityService.GetAllMyCommunities(userId);
                }
                catch(Exception ex)
                {
                    return StatusCode(500, "smth went wrong");
                }
            }
        }
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunityInfo(Guid id)
    {
        try
        {
            var response = await _communityService.GetCommunityInfo(id);
            return Ok(response);
        }
        catch (Exception ex) {
            return StatusCode(500, "Went wrong");
        }
    }

    [HttpGet("{id}/post")]
    public async Task<ActionResult<PostPagedListDto>> GetCommunityPosts(Guid id, [FromQuery] QueryParametersCommunity parametersCommunity)
    {
        var tagIds = parametersCommunity.Tags;
        var sorting = parametersCommunity.Sorting;
        var page = parametersCommunity.CurrentPage;
        var size = parametersCommunity.PageSize;
        try
        {
            var response = await _communityService.GetCommunityPosts(id, tagIds, sorting, page, size);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }

    [Authorize]
    [HttpPost("{id}/post")]
    public async Task<ActionResult<Guid>> CreatePostInCommunity(Guid id, CreatePostDto model)
    {
        Guid? userId = (Guid)HttpContext.Items["userId"];

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        if (userId != null)
        {
            try
            {
                var response = await _communityService.CreatePostInCommunity(id, userId.Value, model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        else
        {
            return Unauthorized();
        }
    }
    
    [Authorize]
    [HttpGet("{id}/role")]
    
    public async Task<ActionResult<string>> GetCommunityRole(Guid id)
    {
        Guid? userId = (Guid)HttpContext.Items["userId"];
        if (userId != null)
        {
            try
            {
                var response = await _communityService.GetCommunityRole(id, userId);
                return Ok(response);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        else
        {
            return Unauthorized();
        }
    }

    [Authorize]
    [HttpPost("{id}/subscribe")]
    public async Task<ActionResult> Subscribe(Guid id)
    {
        Guid? userId = (Guid)HttpContext.Items["userId"];
        if (userId != null)
        {
            try
            {
                await _communityService.Subscribe(id, userId.Value);
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        else
        {
            return Unauthorized();
        }
    }
    
    [Authorize]
    [HttpDelete("{id}/unsubscribe")]
    public async Task<ActionResult> Unsubscribe(Guid id)
    {
        Guid? userId = (Guid)HttpContext.Items["userId"];
        if (userId != null)
        {
            try
            {
                await _communityService.Unsubscribe(id, userId.Value);
                return Ok();
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }
        else
        {
            return Unauthorized();
        }
    }
}