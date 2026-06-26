using AutoMapper;
using MediatR;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.WorkSpaceMembers.Dto;

namespace TaskBoard.Application.WorkSpaceMembers.Queries;

public class GetWorkspaceMembersQuery : IRequest<List<WorkspaceMemberDto>>
{
    public Guid WorkspaceId { get; set; }

    public class Handler(IWorkspaceMemberRepository workspaceMemberRepository, IMapper mapper) : IRequestHandler<GetWorkspaceMembersQuery, List<WorkspaceMemberDto>>
    {
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IMapper _mapper = mapper ;

        public async Task<List<WorkspaceMemberDto>> Handle(GetWorkspaceMembersQuery request, CancellationToken cancellationToken)
        {
            var workspaceMembers = await _workspaceMemberRepository.GetWorkspaceMembersAsync(request.WorkspaceId);

            return  _mapper.Map<List<WorkspaceMemberDto>>(workspaceMembers);    
        }
    }
}