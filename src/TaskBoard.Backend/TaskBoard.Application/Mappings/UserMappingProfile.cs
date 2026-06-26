using AutoMapper;
using TaskBoard.Application.User.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class UserMappingProfile: Profile
{
    public UserMappingProfile()
    {
        CreateMap<ApplicationUser, UserDto>();
        CreateMap<UserDto, ApplicationUser>();

    }
}
