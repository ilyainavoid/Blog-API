using BlogApi.Models.DTO;
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
}