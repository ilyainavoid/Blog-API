using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Repositories.Interfaces;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace BlogApi.Services.Communities;

public class CommunityService : ICommunityService
{
    private readonly IBaseRepository<Community> _communityRepository;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;

    public CommunityService(IBaseRepository<Community> communityRepository, AppDbContext context, IMapper mapper)
    {
        _communityRepository = communityRepository;
        _mapper = mapper;
        _context = context;
    }

    public async Task<List<CommunityDto>> GetAllCommunities()
    {
        var communities = await _communityRepository.GetAll();
        var communitiesDto = _mapper.Map<List<CommunityDto>>(communities);
        return communitiesDto;
    }

    public async Task<List<CommunityUserDto>> GetAllMyCommunities(Guid userId)
    {
        var managedGroups = await _context.CommunitiesAdministrators.Where(admin => admin.UserId == userId).ToListAsync();
        var resultAsAdmin = _mapper.Map<List<CommunityUserDto>>(managedGroups);
        var subscriptions = await _context.CommunitiesSubscribers.Where(subscriber => subscriber.UserId == userId).ToListAsync();
        var resultAsSubscriber = _mapper.Map <List<CommunityUserDto>>(subscriptions);
        var result = resultAsAdmin.Concat<CommunityUserDto>(resultAsSubscriber).ToList();
        return result;
    }

    public async Task<CommunityFullDto> GetCommunityInfo(Guid communityId)
    {
        var communityInfo = await _communityRepository.Get(communityId);
        var result = _mapper.Map<CommunityFullDto>(communityInfo);
        var communityAdmins = await _context.CommunitiesAdministrators
            .Where(community => community.CommunityId == communityId)
            .ToListAsync();
        
        var adminsInfo = await _context.CommunitiesAdministrators
            .Where(community => community.CommunityId == communityId)
            .Select(admin => admin.UserId)
            .ToListAsync();

        var users = await _context.Users
            .Where(user => adminsInfo.Contains(user.Id))
            .ToListAsync();

        var adminsDto = _mapper.Map<List<UserDto>>(users);
        result.Administrators = adminsDto;
        return result;
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