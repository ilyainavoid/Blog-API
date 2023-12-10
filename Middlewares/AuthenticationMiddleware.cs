using BlogApi.Utilities;
using Microsoft.IdentityModel.JsonWebTokens;

namespace BlogApi.Middlewares;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, ITokenUtilities tokenUtilities)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        var token = authHeader?.Split(" ").Last();
        var userId = tokenUtilities.ValidateToken(token);
        if (userId != null)
        {
            context.Items["userId"] = userId;
        }

        await _next(context);
    }
}