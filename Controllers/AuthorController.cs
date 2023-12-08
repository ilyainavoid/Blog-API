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
    public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
    {
        try
        {
            var response = _authorService.GetAuthors();
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}