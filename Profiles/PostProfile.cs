﻿using AutoMapper;
using BlogApi.Models.DTO;
using BlogApi.Models.Entities;

namespace BlogApi.Profiles;

public class PostProfile : Profile
{
    public PostProfile()
    {
        CreateMap<Post, PostDto>();
        CreateMap<Post, PostFullDto>();
    }
}