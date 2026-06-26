using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Boards.Dto;
using TaskBoard.Application.Boards.Helpers;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Commands;

public class CreateBoardCommand : IRequest<BoardDto>
{
    public Guid WorkspaceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceMemberRepository workspaceMemberRepository, IBoardRepository boardRepository, IBoardColumnRepository boardColumnRepository, ILaneRepository laneRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<CreateBoardCommand, BoardDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<BoardDto> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null){
                throw new Exception("User not authenticated");
            }

            var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember)
                throw new Exception("Unauthorized user.");

            var board = new Board
            {
                Id = Guid.NewGuid(),
                WorkspaceId = request.WorkspaceId,
                Name = request.Name,
                Description = request.Description,
                IsOpen = true
            };

            var columns = BoardDefaultGenerator.GenerateDefaultColumns(board.Id);
            var lane = BoardDefaultGenerator.GenerateDefaultLane(board.Id);

            await _boardRepository.AddBoardAsync(board);
            await _boardColumnRepository.AddDefaultColumnsAsync(columns);
            await _laneRepository.AddLaneAsync(lane);
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<BoardDto>(board);
        }
    }
}
