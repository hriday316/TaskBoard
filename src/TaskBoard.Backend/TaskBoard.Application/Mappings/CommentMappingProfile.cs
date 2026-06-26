using System;
using AutoMapper;
using TaskBoard.Application.Comments.Dto;

namespace TaskBoard.Application.Mappings;

public class CommentMappingProfile: Profile
{

    public CommentMappingProfile()
    {
        CreateMap<Domain.Entities.Comment, CommentDto>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.User.ProfilePictureUrl));
    }

}
