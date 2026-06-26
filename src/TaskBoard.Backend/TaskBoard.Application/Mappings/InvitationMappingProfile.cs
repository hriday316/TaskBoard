using System;
using AutoMapper;
using TaskBoard.Application.Invitations.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class InvitationMappingProfile : Profile
{
    public InvitationMappingProfile()
    {
        CreateMap<WorkspaceInvitation, InvitationDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
             
     }

}
