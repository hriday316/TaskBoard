using AutoMapper;
using TaskBoard.Application.Boards.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class BoardMappingProfile : Profile
{
    public BoardMappingProfile()
    {
        CreateMap<Board, BoardDto>();
    }
}
