using AutoMapper;
using TaskBoard.Application.Lanes.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class LaneMappingProfile : Profile
{
    public LaneMappingProfile()
    {
        CreateMap<Lane, LaneDto>();
    }
}
