using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.BoardColumns.Dto;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.BoardColumns.Commands;

public class UpdateBoardColumnCommand : IRequest<BoardColumnDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public Guid ColumnId { get; set; }
    public string Name { get; set; } = string.Empty;

    public class Handler(IHttpContextAccessor httpContextAccessor, IWorkspaceMemberRepository workspaceMemberRepository, IBoardColumnRepository boardColumnRepository, IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<UpdateBoardColumnCommand, BoardColumnDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IMapper _mapper = mapper;
        public async Task<BoardColumnDto> Handle(UpdateBoardColumnCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("User not authenticated");
            }

            var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember)
            {
                throw new Exception("Unauthorized user.");
            }
            var column = await _boardColumnRepository.GetColumnByIdAsync(request.ColumnId);
            if (column == null || column.BoardId != request.BoardId || column.Board.WorkspaceId != request.WorkspaceId)
                throw new Exception("Column not found");

            column.Name = request.Name;
            column.UpdatedAtUtc = DateTime.UtcNow;
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<BoardColumnDto>(column);
        }
    }
}
