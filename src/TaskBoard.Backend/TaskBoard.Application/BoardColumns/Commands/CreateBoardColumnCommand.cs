using System;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.BoardColumns.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.BoardColumns.Commands;

public class CreateBoardColumnCommand : IRequest<BoardColumnDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }
    public string? Color { get; set; }
    public bool IsDefault { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IBoardColumnRepository boardColumnRepository, IBoardRepository boardRepository, IWorkspaceMemberRepository workspaceMemberRepository, IMapper mapper) : IRequestHandler<CreateBoardColumnCommand, BoardColumnDto>
    {
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<BoardColumnDto> Handle(CreateBoardColumnCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("User not authenticated");
            }
            var isWorkspaceMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isWorkspaceMember){
                throw new Exception("Unauthorize user.");
            }

            var board = await boardRepository.GetBoardByIdAsync(request.BoardId);
            if (board == null || board.WorkspaceId != request.WorkspaceId){
                throw new Exception("Board does not belong to this workspace");
            }


            var boardColumn = new BoardColumn
            {
                BoardId = request.BoardId,
                Name = request.Name,
                Order = request.Order,
                ColumnSpan = request.ColumnSpan,
                RowSpan = request.RowSpan,
                Color = request.Color,
                IsDefault = request.IsDefault
            };

              await _boardColumnRepository.CreateColumnAsync(boardColumn);
            return _mapper.Map<BoardColumnDto>(boardColumn);

        }
    }

}
