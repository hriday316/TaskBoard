using System;
using AutoMapper;
using TaskBoard.Application.BoardColumns.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Mappings;

public class BoardColumnMappingProfile : Profile
{
    public BoardColumnMappingProfile()
    {
        CreateMap<BoardColumn, BoardColumnDto>();
    }

}
