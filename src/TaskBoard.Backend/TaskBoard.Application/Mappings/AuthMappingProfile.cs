using System;
using AutoMapper;
using TaskBoard.Application.Auth.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class AuthMappingProfile : Profile
{
    public AuthMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<UserDto, ApplicationUser>();
            //  .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
    }
}
