using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Repositories.Interfaces;
using BlogApi.Services.DbContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Abstractions;
using Microsoft.IdentityModel.Abstractions;

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

    public async Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid>? tagIds, PostSorting? sorting, int page, int size)
    {
        var result = new PostPagedListDto();
        var posts = new List<Post>();

        switch (sorting)
        {
            case PostSorting.CreateAsc:
                posts = await _context.Posts
                    .Where(post => post.CommunityId == communityId)
                    .Include(post => post.Tags)
                    .OrderBy(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.CreateDesc:
                posts = await _context.Posts
                    .Where(post => post.CommunityId == communityId)
                    .Include(post => post.Tags)
                    .OrderByDescending(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeAsc:
                posts = await _context.Posts
                    .Where(post => post.CommunityId == communityId)
                    .Include(post => post.Tags)
                    .OrderBy(post => post.LikesAmount)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeDesc:
                posts = await _context.Posts
                    .Where(post => post.CommunityId == communityId)
                    .Include(post => post.Tags)
                    .OrderByDescending(post => post.LikesAmount)
                    .ToListAsync();
                break;
            

            default:
                posts = await _context.Posts
                    .Where(post => post.CommunityId == communityId)
                    .Include(post => post.Tags)
                    .ToListAsync();
                break;
        }

        if (tagIds != null)
        {
            posts = posts.Where(p => p.Tags.Any(tag => tagIds.Contains(tag.Id))).ToList();
        }
        var count = Math.Ceiling((double)posts.Count / size);
        var pagination = new PageInfoModel(size, (int)count, page);
        var postDtoList = _mapper.Map<List<PostDto>>(posts);
        result.Posts = postDtoList;
        result.Pagination = pagination;
        return result;
    }
    
    public async Task<Guid> CreatePostInCommunity(Guid communityId, Guid userId, CreatePostDto newPost)
    {
        bool isCommunityExists = await _context.Community.AnyAsync(c => c.Id == communityId);
        if (!isCommunityExists)
        {
            throw new Exception("Not found");
        }
        
        bool isAdmin =
            await _context.CommunitiesAdministrators.AnyAsync(
                ca => ca.UserId == userId && ca.CommunityId == communityId);
        if (!isAdmin)
        {
            throw new Exception("Forbidden");
        }

        bool validTags = newPost.Tags.All(tagId => _context.Tags.Any(dbTag => dbTag.Id == tagId));

        if (!validTags)
        {
            throw new Exception("Not Found");
        }

        List<Tag> newPostTags = await _context.Tags.Where(tag => newPost.Tags.Contains(tag.Id)).ToListAsync();

        var post = new Post
        {
            CreateTime = DateTime.UtcNow,
            Title = newPost.Title,
            Description = newPost.Description,
            ReadingTime = newPost.ReadingTime,
            Image = newPost.Image,
            AuthorId = userId,
            Author = await _context.Users
                .Where(user => user.Id == userId)
                .Select(user => user.FullName)
                .FirstOrDefaultAsync(),
            CommunityId = communityId,
            CommunityName = await _context.Community
                .Where(community => community.Id == communityId)
                .Select(community => community.Name)
                .FirstOrDefaultAsync(),
            AddressId = newPost.AddressId,
            Tags = newPostTags
        };
        
        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
        return post.Id;
    }

    public async Task<string> GetCommunityRole(Guid communityId, Guid? userId)
    {
        bool isCommunityExists = await _context.Community.AnyAsync(c => c.Id == communityId);
        if (isCommunityExists)
        {
            if (await _context.CommunitiesAdministrators.AnyAsync(adm => adm.UserId == userId))
            {
                return CommunityRole.Administrator.ToString();
            }
            else if (await _context.CommunitiesSubscribers.AnyAsync(sub => sub.UserId == userId))
            {
                return CommunityRole.Subscriber.ToString();
            }
            else
            {
                return "null";
            }
        }
        else
        {
            throw new Exception($"Community with id={communityId.ToString()} does not exists");
        }
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