using System;
using AutoMapper;
using TaskBoard.Application.Assignments.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class AssignmentMappingProfile : Profile

{
    public AssignmentMappingProfile()
    {
        CreateMap<CardAssignment,  AssignmentDto>()
            .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.WorkspaceMemberId));
    }

}
