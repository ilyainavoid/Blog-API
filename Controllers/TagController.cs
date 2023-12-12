using BlogApi.Models.DTO;
using BlogApi.Services.Tags;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/tag")]
[ApiController]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;

    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTags()
    {
        try
        {
            var response = await _tagService.GetAllTags();
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal Server Error"
            };
            return StatusCode(500, response);
        }
    }
}