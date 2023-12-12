using System.Net;
using System.Security.Claims;
using BlogApi.Exceptions;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/account")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;


    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] UserRegisterModel userRegisterModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _userService.Register(userRegisterModel);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal server error"
            };
            return StatusCode(500, response);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenResponse>> Login([FromBody] LoginCredentials loginCredentials)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var response = await _userService.Login(loginCredentials);
            return Ok(response);
        }
        catch (BadRequestException exception)
        {
            var response = new Response
            {
                Status = "Login failed",
                Message = exception.Message
            };
            return StatusCode(400, response);
        }
        catch (Exception exception)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal server error"
            };
            return StatusCode(500, response);
        }
    }
    
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult<Response>> Logout()
    {
        var authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();

        if (authorizationHeader == null)
        {
            return BadRequest("Authorization header is missing");
        }

        var token = authorizationHeader.Split(" ").Last();
        try
        {
            await _userService.Logout(token);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {

        var idClaim = HttpContext.User.FindFirst("id");
        UserDto response = new UserDto();
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.Value, out var userId))
            {
                response = await _userService.GetProfileInfo(userId);
            }
        }
        return Ok(response);
    }
    
    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> EditProfile([FromBody] UserEditModel userEditModel)
    {

        var idClaim = HttpContext.User.FindFirst("id");
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.Value, out var userId))
            {
                await _userService.EditProfileInfo(userId, userEditModel);
            }
        }
        return Ok();
    }
}