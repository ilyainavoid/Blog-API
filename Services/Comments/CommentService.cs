using AutoMapper;
using BlogApi.Exceptions;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Services.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApi.Services.Comments;

public class CommentService : ICommentService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public CommentService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<CommentDto>> GetTree(Guid id, Guid userId)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        var parentComment = await _context.Comments.Include(c => c.ChildComments).FirstOrDefaultAsync(comment => comment.Id == id);
        //Check if exists
        if (parentComment == null)
        {
            throw new NotFoundException($"Comment with id={id} not found in  database");
        }
        //Check if is the comment is root comment
        if (parentComment.ParentCommentId != null)
        {
            throw new BadRequestException($"Comment with id={id} is not a root element");
        }
        //Check if has access
        var post = await _context.Posts.FindAsync(parentComment.PostId);
        if (post.CommunityId != null)
        {
            bool isSubscriber = _context.CommunitiesSubscribers.Any(cs =>
                cs.CommunityId == post.CommunityId && cs.UserId == userId);
            
            bool isAdmin = _context.CommunitiesAdministrators.Any(cs =>
                cs.CommunityId == post.CommunityId && cs.UserId == userId);

            if (!isAdmin && !isSubscriber)
            {
                throw new ForbiddenException("User has no access to this comment");
            }
        }

        //Get replies
        var childComments = parentComment.ChildComments;
        var result = new List<CommentDto>();
        //Creating an answer using recursion 
        foreach (var childComment in childComments)
        {
            //Get child with navigation property
            var commentWithChildren = _context.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefault(c => c.Id == childComment.Id);
            var commentDto = _mapper.Map<CommentDto>(commentWithChildren);
            result.Add(commentDto);
            if (commentWithChildren.ChildComments.Count != 0)
            {
                await AddChildren(result, childComment.ChildComments);
            }
        }

        return result;
    }

    public async Task AddChildren(List<CommentDto> result, List<Comment> children)
    {
        foreach (var childComment in children)
        {
            var commentWithChildren = _context.Comments
                .Include(c => c.ChildComments)
                .FirstOrDefault(c => c.Id == childComment.Id);
            var commentDto = _mapper.Map<CommentDto>(commentWithChildren);
            result.Add(commentDto);
            if (commentWithChildren.ChildComments.Count != 0)
            {
                await AddChildren(result, childComment.ChildComments);
            }
        }
    }

    public async Task AddComment(Guid id, Guid userId, CreateCommentDto commentModel)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        //Check if post exists
        bool postExists = await _context.Posts.AnyAsync(post => post.Id == id);
        if (!postExists)
        {
            throw new NotFoundException($"Post with id={id} is not found in database");
        }

        //Check if post has a parent id specified
        if (commentModel.ParentId != null)
        {
            //Check if parent exists
            bool parentExists = await _context.Comments.AnyAsync(c => c.Id == commentModel.ParentId);
            if (!parentExists)
            {
                throw new NotFoundException($"Comment with id={commentModel.ParentId} is not found in database");
            }
            
            //Check if parent comment belongs to the same post
            var parentComment = await _context.Comments.FindAsync(commentModel.ParentId);
            if (id != parentComment.PostId)
            {
                throw new BadRequestException($"Incorrect combination between post with id={id} and parent comment with id={commentModel.ParentId}");
            }
        }
        
        var post = await _context.Posts.FindAsync(id);
        var communityId = post.CommunityId;

        if (communityId != null)
        {
            //Check if user has access to the post
            bool hasAccess =
                await _context.CommunitiesAdministrators.AnyAsync(
                    admin => admin.UserId == userId && admin.CommunityId == communityId);

            if (!hasAccess)
            {
                throw new ForbiddenException("Forbidden");
            }
        }

        //Create new comment and insert
        var newComment = new Comment
        {
            CreateTime = DateTime.UtcNow,
            Content = commentModel.Content,
            AuthorId = userId,
            AuthorName = _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.FullName)
                .FirstOrDefault(),
            SubComments = 0,
            ParentCommentId = commentModel.ParentId,
            PostId = id
        };
        await _context.Comments.AddAsync(newComment);

        await _context.SaveChangesAsync();
    }

    public async Task EditComment(Guid id, Guid userId, UpdateCommentDto commentModel)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        var comment = await _context.Comments.FindAsync(id);

        //Check if comment exists
        if (comment == null)
        {
            throw new NotFoundException($"Comment with id={id} is not found in database");
        }

        //Check if user has access to the comment
        if (comment.AuthorId != userId)
        {
            throw new ForbiddenException("Forbidden");
        }

        //Update data of comment
        comment.Content = commentModel.Content;
        comment.ModifiedDate = DateTime.UtcNow;
        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteComment(Guid id, Guid userId)
    {
        if (userId == default)
        {
            throw new UnauthorizedException("User is unauthorized");
        }

        var comment = await _context.Comments.Include(c => c.ChildComments).FirstOrDefaultAsync(c => c.Id == id);

        //Check if comment exists
        if (comment == null)
        {
            throw new NotFoundException($"Comment with id={id} is not found in database");
        }

        //Check if user has access to the comment
        if (comment.AuthorId != userId)
        {
            throw new ForbiddenException("Forbidden");
        }

        //If there aren't any replies
        if (comment.ChildComments.Count == 0)
        {
            //Delete the comment
            _context.Comments.Remove(comment);
        }
        //If there're replies
        else
        {
            comment.Content = "";
            comment.DeleteDate = DateTime.UtcNow;
            _context.Comments.Update(comment);
        }
        await _context.SaveChangesAsync();
    }
}