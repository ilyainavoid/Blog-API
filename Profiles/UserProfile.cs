using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;
using BlogApi.Repositories;

namespace BlogApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegisterModel, User>();
            CreateMap<UserDto, User>();
            CreateMap<UserEditModel, User>();
        }
    }
}
