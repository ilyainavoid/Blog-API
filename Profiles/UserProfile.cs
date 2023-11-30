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
            CreateMap<User, UserDto>();
            CreateMap<UserEditModel, User>();
        }
    }
}
