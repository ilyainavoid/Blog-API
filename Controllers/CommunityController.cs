using BlogApi.Exceptions;
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
            var response = await _communityService.GetAllMyCommunities(userId);
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

    [HttpGet("{id}")]
    public async Task<ActionResult<CommunityFullDto>> GetCommunityInfo(Guid id)
    {
        try
        {
            var response = await _communityService.GetCommunityInfo(id);
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

    [HttpGet("{id}/post")]
    public async Task<ActionResult<PostPagedListDto>> GetCommunityPosts(Guid id, [FromQuery] QueryParametersCommunity parametersCommunity)
    {
        var tagIds = parametersCommunity.Tags;
        var sorting = parametersCommunity.Sorting;
        var page = parametersCommunity.CurrentPage;
        var size = parametersCommunity.PageSize;

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
            var response = await _communityService.GetCommunityPosts(id, userId, tagIds, sorting, page, size);
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
    [HttpPost("{id}/post")]
    public async Task<ActionResult<Guid>> CreatePostInCommunity(Guid id, CreatePostDto model)
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

        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        try
        {
            var response = await _communityService.CreatePostInCommunity(id, userId, model);
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
    [HttpGet("{id}/role")]
    
    public async Task<ActionResult<string>> GetCommunityRole(Guid id)
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
            var response = await _communityService.GetCommunityRole(id, userId);
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
    [HttpPost("{id}/subscribe")]
    public async Task<ActionResult> Subscribe(Guid id)
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
            await _communityService.Subscribe(id, userId);
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
    [HttpDelete("{id}/unsubscribe")]
    public async Task<ActionResult> Unsubscribe(Guid id)
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
            await _communityService.Unsubscribe(id, userId);
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