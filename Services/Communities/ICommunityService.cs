using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Models.Enums;

namespace BlogApi.Services.Communities;

public interface ICommunityService
{
    Task<List<CommunityDto>> GetAllCommunities();
    Task<List<CommunityUserDto>> GetAllMyCommunities(Guid userId);
    Task<List<CommunityFullDto>> GetCommunityInfo(Guid communityId);
    Task<List<PostPagedListDto>> GetCommunityPosts(Guid communityId, List<Tag> tags, PostSorting sorting, int page, int size);
    Task<Guid> CreatePostInCommunity(Guid communityId);
    Task<CommunityRole> GetCommunityRole(Guid communityId);
    Task Subscribe(Guid communityId);
    Task Unsubscribe(Guid communityId);
}