using AutoMapper;
using BlogApi.Exceptions;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using BlogApi.Services.Communities;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Services.Posts;

public class PostService : IPostService {

    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICommunityService _communityService;
    public PostService(AppDbContext context, IMapper mapper, ICommunityService communityService)
    {
        _context = context;
        _mapper = mapper;
        _communityService = communityService;
    }
    
    public async Task<PostPagedListDto> GetPosts(Guid userId, QueryParametersPost parametersPost)
    {
        bool isAuthorized = userId != default;
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
                    .Include(p => p.Comments)
                    .OrderBy(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.CreateDesc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .Include(p => p.Comments)
                    .OrderByDescending(post => post.CreateTime)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeAsc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .Include(p => p.Comments)
                    .OrderBy(post => post.LikesAmount)
                    .ToListAsync();
                break;
            
            case PostSorting.LikeDesc:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .Include(p => p.Comments)
                    .OrderByDescending(post => post.LikesAmount)
                    .ToListAsync();
                break;
            

            default:
                posts = await _context.Posts
                    .Include(post => post.Tags)
                    .Include(p => p.Comments)
                    .ToListAsync();
                break;
        }

        if (parametersPost.MinReadingTime != 0)
        {
            posts = posts.Where(post => post.ReadingTime >= minReadingTime).ToList();
        }

        if (parametersPost.MaxReadingTime != 0)
        {
            posts = posts.Where(post => post.ReadingTime <= maxReadingTime).ToList();
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
        
        //Логика для авторизованного пользователя
        if (isAuthorized)
        {
          
            var mySubscriptions = await _context.CommunitiesSubscribers.Where(cs => cs.UserId == userId)
                .Select(cs => cs.CommunityId).ToListAsync();

            var myManagedCommunities = await _context.CommunitiesAdministrators.Where(ca => ca.UserId == userId)
                .Select(ca => ca.CommunityId).ToListAsync();

            var myCommunities = myManagedCommunities.Union(mySubscriptions).ToList();
                
            if (parametersPost.OnlyMyCommunities && myCommunities != null)
            {
                posts = posts.Where(p => myCommunities.Any(mc => mc == p.CommunityId)).ToList();
            }
            else if (!parametersPost.OnlyMyCommunities && myCommunities != null)
            {
                posts = posts
                    .Where(p => (p.CommunityId == null) ||
                                (_context.Community.Any(c => c.Id == p.CommunityId && !c.IsClosed)) ||
                                myCommunities.Any(mc => mc == p.CommunityId && (_context.Community.Any(c => c.Id == mc && c.IsClosed))))
                    .ToList();
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
        //Логика для неавторизованного пользователя
        else
        {
            posts = posts
                .Where(p => p.CommunityId == null ||
                            (_context.Community.Any(c => c.Id == p.CommunityId && !c.IsClosed)))
                .ToList();
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

    public async Task<Guid> CreatePersonalPost(Guid userId, CreatePostDto model)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        bool validTags = model.Tags.All(tagId => _context.Tags.Any(dbTag => dbTag.Id == tagId));
        if (!validTags)
        {
            throw new NotFoundException("NotFound");
        }
        List<Tag> newPostTags = await _context.Tags.Where(tag => model.Tags.Contains(tag.Id)).ToListAsync();
        var newPost = new Post
        {
            CreateTime = DateTime.UtcNow,
            Title = model.Title,
            Description = model.Description,
            ReadingTime = model.ReadingTime,
            Image = model.Image,
            AuthorId = userId,
            Author = await _context.Users
                .Where(user => user.Id == userId)
                .Select(user => user.FullName)
                .FirstOrDefaultAsync(),
            AddressId = model.AddressId,
            Tags = newPostTags,
            CommunityId = null,
            CommunityName = null
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();
        return newPost.Id;
    }

    public async Task<PostFullDto> GetPostInfo(Guid userId, Guid postId)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        //Get the post
        var postInfo = await _context.Posts
            .Include(p => p.Tags)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == postId);
        var result = new PostFullDto();

        //Check if the post is found
        if (postInfo == null)
        {
            throw new NotFoundException($"Post with id={postId.ToString()} is not found");
        }

        //Check if the post is personal
        bool isPersonal = postInfo.CommunityId == null;

        //Determine post's parameters 
        postInfo.HasLike = await _context.Likes.AnyAsync(like => like.AuthorId == userId && like.PostId == postId);
        postInfo.CommentsCount = postInfo.Comments.Count();
        postInfo.LikesAmount = postInfo.Likes.Count();


        //Post is in some community
        if (!isPersonal)
        {
            var community = await _context.Community.FindAsync(postInfo.CommunityId);
            bool isClosed = community.IsClosed;

            //Post is in a closed community
            if (isClosed)
            {
                string role = await _communityService.GetCommunityRole(postInfo.CommunityId.Value, userId);

                //User isn't in the closed community
                if (role == "null")
                {
                    throw new ForbiddenException(
                        $"Access to closed community post with id={postInfo.CommunityId.ToString()} is forbidden");
                }

                result = _mapper.Map<PostFullDto>(postInfo);
                result.Tags = _mapper.Map<List<TagDto>>(postInfo.Tags);

                //Add comments to a result
                var comments = postInfo.Comments;
                var commentsToPost = new List<CommentDto>();
                foreach (var comment in comments)
                {
                    //Take comments that are not reply-comments
                    if (comment.ParentCommentId == null)
                    {
                        var commentDto = _mapper.Map<CommentDto>(comment);
                        commentsToPost.Add(commentDto);
                    }
                }
                result.Comments = _mapper.Map<List<CommentDto>>(commentsToPost);
            }
            //Post is in a public community
            else
            {
                result = _mapper.Map<PostFullDto>(postInfo);
                result.Tags = _mapper.Map<List<TagDto>>(postInfo.Tags);
                
                //Add comments to a result
                var comments = postInfo.Comments;
                var commentsToPost = new List<CommentDto>();
                foreach (var comment in comments)
                {
                    //Take comments that are not reply-comments
                    if (comment.ParentCommentId == null)
                    {
                        var commentDto = _mapper.Map<CommentDto>(comment);
                        commentsToPost.Add(commentDto);
                    }
                }
                result.Comments = _mapper.Map<List<CommentDto>>(commentsToPost);
            }
        }
        //Post is personal
        else
        {
            result = _mapper.Map<PostFullDto>(postInfo);
            result.Tags = _mapper.Map<List<TagDto>>(postInfo.Tags);
                
            //Add comments to a result
            var comments = postInfo.Comments;
            var commentsToPost = new List<CommentDto>();
            foreach (var comment in comments)
            {
                //Take comments that are not reply-comments
                if (comment.ParentCommentId == null)
                {
                    var commentDto = _mapper.Map<CommentDto>(comment);
                    commentsToPost.Add(commentDto);
                }
            }
            result.Comments = _mapper.Map<List<CommentDto>>(commentsToPost);
        }

        return result;
    }

    public async Task AddLikeToPost(Guid userId, Guid postId)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        //Check if the post is found
        bool isExist = await _context.Posts.AnyAsync(post => post.Id == postId);
        if (!isExist)
        {
            throw new NotFoundException($"Post with id={postId.ToString()} is not found");
        }
        
        //Get the post
        var postInfo = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        //Check if user has already liked the post
        bool hasLike = await _context.Likes.AnyAsync(like => like.AuthorId == userId && like.PostId == postId);
        if (hasLike)
        {
            throw new BadRequestException($"Post with id={postId.ToString()} has already been liked");
        }
        
        //Check if user has access to the post
        var communityId = postInfo.CommunityId;
        if (communityId.HasValue) //Post is in some community
        {
            var community = await _context.Community.FindAsync(communityId.Value);
            if (community.IsClosed == true) //Post is in a closed community
            {
                var role = await _communityService.GetCommunityRole(communityId.Value, userId);
                if (role == "null") //User has access to the posts of the closed community
                {
                    throw new ForbiddenException(
                        $"Access to closed community post with id={postInfo.CommunityId.ToString()} is forbidden");
                }
            }
        }
        
        //Create a new like
        var like = new Like
        {
            AuthorId = userId,
            PostId = postId
        };

        //Add a like to the db table
        await _context.Likes.AddAsync(like);

        //Update likes counter in the post table
        postInfo.LikesAmount += 1;
        postInfo.HasLike = true;
        _context.Posts.Update(postInfo);

        //Save changes
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLikeFromPost(Guid userId, Guid postId)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        //Check if the post is found
        bool isExist = await _context.Posts.AnyAsync(post => post.Id == postId);
        if (!isExist)
        {
            throw new NotFoundException($"Post with id={postId.ToString()} is not found");
        }
        
        //Get the post
        var postInfo = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
        
        //Check if user hasn't liked the post
        bool hasLike = await _context.Likes.AnyAsync(like => like.AuthorId == userId && like.PostId == postId);
        if (!hasLike)
        {
            throw new BadRequestException($"Post with id={postId.ToString()} hasn't been liked");
        }
        
        //Check if user has access to the post
        var communityId = postInfo.CommunityId;
        if (communityId.HasValue) //Post is in some community
        {
            var community = await _context.Community.FindAsync(communityId.Value);
            if (community.IsClosed == true) //Post is in a closed community
            {
                var role = await _communityService.GetCommunityRole(communityId.Value, userId);
                if (role == "null") //User has access to the posts of the closed community
                {
                    throw new ForbiddenException(
                        $"Access to closed community post with id={postInfo.CommunityId.ToString()} is forbidden");
                }
            }
        }

        //Delete the like from the db table
        var like = await _context.Likes.FirstOrDefaultAsync(like => like.AuthorId == userId && like.PostId == postId);
        _context.Likes.Remove(like);

        //Update likes counter in the post table
        postInfo.LikesAmount -= 1;
        postInfo.HasLike = false;
        _context.Posts.Update(postInfo);

        //Save changes
        await _context.SaveChangesAsync();
    }
}