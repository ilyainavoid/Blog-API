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
            var response = new Response
            {
                Status = "Error occured",
                Message = "Internal server error"
            };
            return StatusCode(500, response);
        }
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult> GetProfile()
    {
        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.ToString(), out var Id))
            {
                userId = Id;
            }
        }
        try
        {
            var response = await _userService.GetProfileInfo(userId);
            return Ok(response);
        }
        catch (UnauthorizedException ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(401, response);
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
    
    [Authorize]
    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> EditProfile([FromBody] UserEditModel userEditModel)
    {
        var idClaim = HttpContext.Items["userId"];
        Guid userId = default;
        if (idClaim != null) {

            if (Guid.TryParse(idClaim.ToString(), out var id))
            {
                userId = id;
            }
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            await _userService.EditProfileInfo(userId, userEditModel);
            var response = new Response
            {
                Status = "Success",
                Message = "User's profile has been updated"
            };
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
}