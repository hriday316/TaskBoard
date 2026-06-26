using System;
using AutoMapper;
using TaskBoard.Application.Assignments.Dto;
using TaskBoard.Application.Cards.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class CardMappingProfile : Profile
{
    public CardMappingProfile()
    {
        CreateMap<Card, CardDto>()
            .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));
        
        CreateMap<CardAssignment, AssignmentDto>()
            .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.WorkspaceMemberId))
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.WorkspaceMember.UserId))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.WorkspaceMember.User.Name))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.WorkspaceMember.User.ProfilePictureUrl));


    }

}
