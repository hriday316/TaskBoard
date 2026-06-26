using MediatR;
using TaskBoard.Application.Tests;
using TaskBoard.Domain.Entities;
using AutoMapper;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Tests.Commands
{
    public class CreateTestCommand : IRequest<TestDto>
    {
        public TestDto Test { get; set; } = null!;
        
        public class Handler : IRequestHandler<CreateTestCommand, TestDto>
        {
            private readonly ITestRepository _repo;
            private readonly IMapper _mapper;

            public Handler(ITestRepository repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<TestDto> Handle(CreateTestCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Test>(request.Test);
                entity.Id = await _repo.CreateAsync(entity);

                return _mapper.Map<TestDto>(entity);
            }
        }
    }
}
