using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BlogApi.Services.Communities;

public class CommunityService : ICommunityService
{
    private readonly IBaseRepository<Community> _communityRepository;
    private readonly IMapper _mapper;

    public CommunityService(IBaseRepository<Community> communityRepository, IMapper mapper)
    {
        _communityRepository = communityRepository;
        _mapper = mapper;
    }

    public async Task<List<CommunityDto>> GetAllCommunities()
    {
        var communities = await _communityRepository.GetAll();
        var communitiesDto = _mapper.Map<List<CommunityDto>>(communities);
        return communitiesDto;
    }

    public Task<List<CommunityUserDto>> GetAllMyCommunities()
    {
        throw new NotImplementedException();
    }

    public Task<List<CommunityFullDto>> GetCommunityInfo(Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task<List<PostPagedListDto>> GetCommunityPosts(Guid communityId, List<Tag> tags, PostSorting sorting, int page, int size)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> CreatePostInCommunity(Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task<CommunityRole> GetCommunityRole(Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task Subscribe(Guid communityId)
    {
        throw new NotImplementedException();
    }

    public Task Unsubscribe(Guid communityId)
    {
        throw new NotImplementedException();
    }
}