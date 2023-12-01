using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class CommunityProfile : Profile
{
    public CommunityProfile()
    {
        CreateMap<Community, CommunityDto>();
    }
}