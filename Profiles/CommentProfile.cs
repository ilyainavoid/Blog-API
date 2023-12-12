using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.AuthorName))
            .ForMember(dest => dest.SubComments, opt => opt.MapFrom(src => src.ChildComments.Count));
    }
}