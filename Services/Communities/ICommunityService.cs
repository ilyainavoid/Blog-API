using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Services.Communities;

public interface ICommunityService
{
    Task<List<CommunityDto>> GetAllCommunities();
    Task<List<CommunityUserDto>> GetAllMyCommunities(Guid userId);
    Task<CommunityFullDto> GetCommunityInfo(Guid communityId);
    Task<PostPagedListDto> GetCommunityPosts(Guid communityId, List<Guid>? tags, PostSorting? sorting, int page, int size);
    Task<Guid> CreatePostInCommunity(Guid communityId, Guid userId, CreatePostDto newPost);
    Task<string> GetCommunityRole(Guid communityId, Guid? userId);
    Task Subscribe(Guid communityId);
    Task Unsubscribe(Guid communityId);
}