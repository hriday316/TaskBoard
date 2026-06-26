using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Lanes.Dto;

namespace TaskBoard.Application.Lanes.Commands;

public class UpdateLaneCommand : IRequest<LaneDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public Guid LaneId { get; set; }
    public string Name { get; set; } = string.Empty;

    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceMemberRepository workspaceMemberRepository, ILaneRepository laneRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<UpdateLaneCommand, LaneDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<LaneDto> Handle(UpdateLaneCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User is not authenticated.");
            var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember){
                throw new Exception("Unauthorized user.");
            }

            var lane = await _laneRepository.GetLaneByIdAsync(request.LaneId);
            if (lane == null || lane.BoardId != request.BoardId || lane.Board.WorkspaceId != request.WorkspaceId)
            {
                throw new Exception("Lane not found");
            }

            lane.Name = request.Name;
            lane.UpdatedAtUtc = DateTime.UtcNow;
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<LaneDto>(lane);
        }
    }
}
