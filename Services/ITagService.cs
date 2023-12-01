using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Services;

public interface ITagService
{
    Task<List<TagDto>> GetAllTags();
}