using BlogApi.Models.DTO;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Authors;

public class AuthorService : IAuthorService
{
    private readonly AppDbContext _context;

    public AuthorService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<AuthorDto>> GetAuthors()
    {
        var result = new List<AuthorDto>();

        //Take authors from users
        var authors = await _context.Users
            .Where(user => user.IsAuthor == true)
            .Include(user => user.Posts)
            .ToListAsync();

        //Prepare result list
        foreach (var user in authors)
        {
            //Calculate total amount of likes
            int likesCount = _context.Likes.AsEnumerable().Count(like => user.Posts.Any(post => post.Id == like.PostId && post.AuthorId == user.Id));

            //Map fields from user to author
            var author = new AuthorDto
            {
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Posts = user.Posts.Count(),
                Likes = likesCount,
                Created = user.CreateTime
            };
            
            //Add author to the result list
            result.Add(author);
        }

        return result;
    }
}