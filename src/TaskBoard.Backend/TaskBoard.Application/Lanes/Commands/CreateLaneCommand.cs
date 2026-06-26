using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Lanes.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Lanes.Commands;

public class CreateLaneCommand : IRequest<LaneDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? Color { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, ILaneRepository laneRepository, IBoardRepository boardRepository, IWorkspaceMemberRepository workspaceMemberRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<CreateLaneCommand, LaneDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<LaneDto> Handle(CreateLaneCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new Exception("User not authenticated");

              var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember)
                throw new Exception("Unauthorized user.");

            var board = await _boardRepository.GetBoardByIdAsync(request.BoardId);
            if (board == null || board.WorkspaceId != request.WorkspaceId)
                throw new Exception("Board does not belong to this workspace");

            var lane = new Lane
            {
                Id = Guid.NewGuid(),
                BoardId = request.BoardId,
                Name = request.Name,
                Order = request.Order,
                Color = request.Color
            };

            await _laneRepository.AddLaneAsync(lane);
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<LaneDto>(lane);
        }
    }
}
