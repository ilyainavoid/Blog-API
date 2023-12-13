using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>()
            .ForMember(dest => dest.CommentsCount, opt => opt.MapFrom(src => src.Comments.Count));
        CreateMap<Post, PostFullDto>();
    }
}