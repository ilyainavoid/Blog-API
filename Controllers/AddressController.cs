using BlogApi.Exceptions;
using BlogApi.Models.DTO;
using BlogApi.Services.Address;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly IAddressService _addressService;

    public AddressController(IAddressService addressService)
    {
        _addressService = addressService;
    }

    [HttpGet("search")]
    public async Task<ActionResult> Search([FromQuery] long? parentId, [FromQuery] string? query)
    {
        try
        {
            var response = await _addressService.Search(parentId, query);
            return Ok(response);
        }
        catch (Exception ex)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = ex.Message
            };
            return StatusCode(500, response);
        }
    }
    
    [HttpGet("chain")]
    public async Task<ActionResult> GetChain([FromQuery] Guid objectGuid)
    {
        try
        {
            var response = await _addressService.GetChain(objectGuid);
            return Ok(response);
        }
        catch (NotFoundException exception)
        {
            var response = new Response
            {
                Status = "Error occured",
                Message = exception.Message
            };
            return StatusCode(404, response);
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