using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Posts;

public class PostService : IPostService {

    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public PostService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<PostPagedListDto> GetPosts(Guid? userId, QueryParametersPost parametersPost)
    {
        bool isAuthorized = userId != null;
        var posts = new List<Post>();
        var postDtos = new List<PostDto>();

        var sorting = parametersPost.Sorting;
        var minReadingTime = parametersPost.MinReadingTime;
        var maxReadingTime = parametersPost.MaxReadingTime;
        
        switch (sorting)
        {
            case PostSorting.CreateAsc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .OrderBy(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.CreateDesc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .OrderByDescending(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeAsc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .OrderBy(post => post.LikesAmount)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeDesc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .OrderByDescending(post => post.LikesAmount)
                    .ToListAsync();
                break;
            

            default:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .ToListAsync();
                break;
        }

        if (parametersPost.MinReadingTime != 0 && parametersPost.MaxReadingTime != 0)
        {
            posts = posts.Where(post => post.ReadingTime >= minReadingTime && post.ReadingTime <= maxReadingTime).ToList();
        }

        if (parametersPost.AuthorsName != null)
        {
            string queryName = parametersPost.AuthorsName.ToString().ToLower();
            posts = posts.Where(post => post.Author.ToString().ToLower().Contains(queryName)).ToList();
        }

        if (parametersPost.Tags != null)
        {
            posts = posts.Where(p => p.Tags.Any(tag => parametersPost.Tags.Contains(tag.Id))).ToList();
        }
        
        if (isAuthorized)
        {
            if (parametersPost.OnlyMyCommunities)
            {
                var mySubscriptions = await _context.CommunitiesSubscribers.Where(cs => cs.UserId == userId)
                    .Select(cs => cs.CommunityId).ToListAsync();

                var myManagedCommunities = await _context.CommunitiesAdministrators.Where(ca => ca.UserId == userId)
                    .Select(ca => ca.CommunityId).ToListAsync();

                var myCommunities = myManagedCommunities.Union(mySubscriptions).ToList();
                
                if (myCommunities != null)
                {
                    posts = posts.Where(p => myCommunities.Any(mc => mc == p.CommunityId)).ToList();
                }
            }

            foreach (var post in posts)
            {
                bool hasLike = _context.Likes.Any(like => like.AuthorId == userId && like.PostId == post.Id);
                if (hasLike)
                {
                    post.HasLike = true;
                }
            }
            
            postDtos = _mapper.Map<List<PostDto>>(posts);
        }
        else
        {
            foreach (var post in posts)
            {
                post.HasLike = false;
            }
            postDtos = _mapper.Map<List<PostDto>>(posts);
        }

        var count = Math.Ceiling((double)postDtos.Count / parametersPost.PageSize);
        var pagination = new PageInfoModel(parametersPost.PageSize, (int)count, parametersPost.CurrentPage);

        int startIndex = (parametersPost.CurrentPage - 1) * parametersPost.PageSize;
        postDtos = postDtos.Skip(startIndex).Take(parametersPost.PageSize).ToList();

        var result = new PostPagedListDto
        {
            Posts = postDtos,
            Pagination = pagination
        };
        
        return result;
    }

    public Task<Guid> CreatePersonalPost(Guid? userId, CreatePostDto model)
    {
        throw new NotImplementedException();
    }

    public Task<PostFullDto> GetPostInfo(Guid? userId, Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task AddLikeToPost(Guid? userId, Guid postId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteLikeFromPost(Guid? userId, Guid postId)
    {
        throw new NotImplementedException();
    }
}