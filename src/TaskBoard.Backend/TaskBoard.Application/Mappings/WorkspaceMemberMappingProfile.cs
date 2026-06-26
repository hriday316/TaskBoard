using System;
using AutoMapper;
using TaskBoard.Application.WorkSpaceMembers.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class WorkspaceMemberMappingProfile : Profile
{
    public WorkspaceMemberMappingProfile()
    {
        CreateMap<WorkspaceMember, WorkspaceMemberDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl));
    }
}
