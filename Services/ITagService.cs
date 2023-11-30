using BlogApi.Models.Entities;

namespace BlogApi.Services;

public interface ITagService
{
    Task<List<Tag>> GetAllTags();
}