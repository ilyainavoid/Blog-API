using BlogApi.Models.DTO;

namespace BlogApi.Services.Authors;

public interface IAuthorService
{
    public Task<List<AuthorDto>> GetAuthors();
}