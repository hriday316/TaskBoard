using AutoMapper;
using MediatR;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Tests.Commands
{
    public class UpdateTestCommand : IRequest<TestDto>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public class Handler : IRequestHandler<UpdateTestCommand, TestDto>
        {
            private readonly ITestRepository _repo;
            private readonly IMapper _mapper;

            public Handler(ITestRepository repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }
            public async Task<TestDto> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
            {
                var entity = _mapper.Map<Domain.Entities.Test>(request);
                await _repo.UpdateAsync(entity);
                return _mapper.Map<TestDto>(entity);
            }
        }

    }
}
