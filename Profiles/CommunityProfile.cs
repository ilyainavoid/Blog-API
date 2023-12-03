using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class CommunityProfile : Profile
{
    public CommunityProfile()
    {
        CreateMap<Community, CommunityDto>();
        CreateMap<Community, CommunityFullDto>();
        CreateMap<CommunitySubscriber, CommunityUserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => 1));
        CreateMap<CommunityAdministrator, CommunityUserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => 0));
    }
}