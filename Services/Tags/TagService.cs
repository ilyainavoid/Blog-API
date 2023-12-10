using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Repositories.Interfaces;
using BlogApi.Services.Tags;

namespace BlogApi.Services.Tags;

public class TagService : ITagService
{
    private readonly IBaseRepository<Tag> _tagRepository;
    private readonly IMapper _mapper;
    
    public TagService(IBaseRepository<Tag> tagRepository, IMapper mapper)
    {
        _tagRepository = tagRepository;
        _mapper = mapper;
    }
    
    public async Task<List<TagDto>> GetAllTags()
    {
        var tags = await _tagRepository.GetAll();
        var tagsDto = _mapper.Map<List<TagDto>>(tags);
        return tagsDto;
    }
}