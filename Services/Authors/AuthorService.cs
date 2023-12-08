using BlogApi.Models.DTO;
using BlogApi.Services.DbContexts;

namespace BlogApi.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _context;

    public AuthorService(AppDbContext context)
    {
        _context = context;
    }
    
    public Task<List<AuthorDto>> GetAuthors()
    {
        throw new NotImplementedException();
    }
}