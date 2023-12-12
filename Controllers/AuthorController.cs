using BlogApi.Models.DTO;
using BlogApi.Services.Authors;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/author")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }
    
    [HttpGet("list")]
    public async Task<ActionResult> GetAuthors()
    {
        try
        {
            var response = await _authorService.GetAuthors();
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