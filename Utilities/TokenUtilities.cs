using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Services.DbContexts;
using Microsoft.IdentityModel.Tokens;

namespace BlogApi.Utilities
{
    public interface ITokenUtilities
    {
        string GenerateToken(User user);
        Guid? ValidateToken(string? token);
    }

    public class TokenUtilities : ITokenUtilities
    {
        private readonly AppDbContext _context;

        public TokenUtilities(AppDbContext context)
        {
            _context = context;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("7sFbGh#2L!p@WmJqNt&v3y$Bdasf89@fasda9");
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "Blog",
                Audience = "JwtUser",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                })
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public Guid? ValidateToken(string? token)
        {
            if (token == null || _context.ExpiredTokens.Any(t => t.Token == token))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = "Blog",
                    ValidateIssuer = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("7sFbGh#2L!p@WmJqNt&v3y$Bdasf89@fasda9")),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    LifetimeValidator = (before, expires, tkn, parameters) =>
                    {
                        var utcNow = DateTime.UtcNow;
                        return before <= utcNow && utcNow < expires;
                    },
                    ValidAudience = "JwtUser",
                    ValidateAudience = true
                }, out SecurityToken validToken);

                var jwtToken = (JwtSecurityToken)validToken;
                var userId = Guid.Parse(jwtToken.Claims.First(claim => claim.Type == "id").Value);
                return userId;
            }
            catch
            {
                return null;
            }
        }
    }
}
