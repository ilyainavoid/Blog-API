using BlogApi.Models.Entities;
using BlogApi.Repositories;
using BlogApi.Repositories.Interfaces;

namespace BlogApi.Services;

public class TagService : ITagService
{
    private readonly IBaseRepository<Tag> _tagRepository;
    
    public TagService(IBaseRepository<Tag> tagRepository)
    {
        _tagRepository = tagRepository;
    }
    
    public async Task<List<Tag>> GetAllTags()
    {
        return await _tagRepository.GetAll();
    }
}