using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Boards.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Boards.Commands;

public class UpdateBoardCommand : IRequest<BoardDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceMemberRepository workspaceMemberRepository, IBoardRepository boardRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<UpdateBoardCommand, BoardDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;           
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<BoardDto> Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null){
                throw new Exception("User not authenticated");
            }

             var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember){
                throw new Exception("Unauthorized user.");
            }

            var board = await _boardRepository.GetBoardByIdAsync(request.BoardId);
            if (board == null || board.WorkspaceId != request.WorkspaceId)
            {
                throw new Exception("Board not found.");
            }

            board.Name = request.Name;
            board.Description = request.Description;
            board.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BoardDto>(board);
        }
    }
}
