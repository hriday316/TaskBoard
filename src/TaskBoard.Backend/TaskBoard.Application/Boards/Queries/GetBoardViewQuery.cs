using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Boards.Dto;

namespace TaskBoard.Application.Boards.Queries;

public class GetBoardViewQuery : IRequest<BoardViewDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IBoardRepository boardRepository) : IRequestHandler<GetBoardViewQuery, BoardViewDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IBoardRepository _boardRepository = boardRepository;

        public async Task<BoardViewDto> Handle(GetBoardViewQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null){
                throw new Exception("User not authenticated");
            }

            var result = await _boardRepository.GetBoardViewAsync(request.WorkspaceId, request.BoardId, Guid.Parse(userId))
                ?? throw new Exception("Board not found");
                
            return result;
        }
    }
}
