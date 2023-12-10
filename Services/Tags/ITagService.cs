using BlogApi.Models.DTO;

namespace BlogApi.Services.Tags;

public interface ITagService
{
    Task<List<TagDto>> GetAllTags();
}