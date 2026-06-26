using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Boards.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Boards.Queries;

public class GetBoardsQuery : IRequest<List<BoardDto>>
{
    public Guid WorkspaceId { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceMemberRepository workspaceMemberRepository, IBoardRepository boardRepository, IMapper mapper) : IRequestHandler<GetBoardsQuery, List<BoardDto>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;       
        private readonly IMapper _mapper = mapper;
        public async Task<List<BoardDto>> Handle(GetBoardsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new Exception("User not authenticated");

            var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember)
                throw new Exception("Unauthorized user.");

            var boards = await _boardRepository.GetBoardsByWorkspaceIdAsync(request.WorkspaceId);
            return _mapper.Map<List<BoardDto>>(boards);
        }
    }
}
