using AutoMapper;
using TaskBoard.Application.Tests;
using TaskBoard.Domain.Entities;


namespace TaskBoard.Application.Mappings
{
    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            CreateMap<TestDto, Test>();
                // .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Test, TestDto>();
        }
    }
}

