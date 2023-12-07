using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>();
    }
}