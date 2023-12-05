using BlogApi.Utilities;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlogApi.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenUtilities _tokenUtilities;

    public AuthenticationMiddleware(RequestDelegate next, ITokenUtilities tokenUtilities)
    {
        _next = next;
        _tokenUtilities = tokenUtilities;
    }

    public async Task Invoke(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.Split(" ").Last();
        var userId = _tokenUtilities.ValidateToken(token);
        if (userId != null)
        {
            context.Items["userId"] = userId;
        }

        await _next(context);
    }
}