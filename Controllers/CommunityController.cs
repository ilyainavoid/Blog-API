using BlogApi.Models.DTO;
using BlogApi.Services.Communities;
using Microsoft.AspNetCore.Mvc;

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
}