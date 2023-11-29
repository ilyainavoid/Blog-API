using BlogApi.Models.DTO;
using BlogApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/account/[action]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;


    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<ActionResult<TokenResponse>> Register([FromBody] UserRegisterModel userRegisterModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _userService.Register(userRegisterModel);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginCredentials loginCredentials)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await _userService.Login(loginCredentials);
        return Ok(response);
    }
}