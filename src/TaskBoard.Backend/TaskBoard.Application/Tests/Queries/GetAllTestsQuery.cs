using AutoMapper;
using MediatR;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Tests.Queries
{
    public class GetAllTestsQuery : IRequest<List<TestDto>>
    {
        public class Handler : IRequestHandler<GetAllTestsQuery, List<TestDto>>
        {
            private readonly ITestRepository _repo;
            private readonly IMapper _mapper;

            public Handler(ITestRepository repo, IMapper mapper)
            {
                _repo = repo;
                _mapper = mapper;
            }

            public async Task<List<TestDto>> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
            {
                var data = await _repo.GetAllAsync();
                return _mapper.Map<List<TestDto>>(data);
            }
        }
    }
}
