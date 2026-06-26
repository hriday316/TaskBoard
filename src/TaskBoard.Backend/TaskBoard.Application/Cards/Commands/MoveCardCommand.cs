using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;

namespace TaskBoard.Application.Cards.Commands;

public class MoveCardCommand : IRequest
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public Guid CardId { get; set; }
    public Guid TargetBoardColumnId { get; set; }
    public Guid TargetLaneId { get; set; }
    public int TargetOrder { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, ICardRepository cardRepository, IBoardColumnRepository boardColumnRepository, ILaneRepository laneRepository, IWorkspaceMemberRepository workspaceMemberRepository, IUnitOfWorkRepository unitOfWorkRepository) : IRequestHandler<MoveCardCommand>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICardRepository _cardRepository = cardRepository;
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;    
        public async Task Handle(MoveCardCommand request, CancellationToken cancellationToken)
        {
            var userId =  _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier)?? throw new Exception("User not authenticated");
     
            var card = await _cardRepository.GetCardForUpdateAsync(request.CardId) ?? throw new Exception("Card not found");

    
            var targetColumn = await _boardColumnRepository.GetColumnByIdAsync(request.TargetBoardColumnId);

            var targetLane = await _laneRepository.GetLaneByIdAsync(request.TargetLaneId);

            if (targetColumn == null || targetColumn.BoardId != request.BoardId){
                throw new Exception("Column does not belong to this board");
            }
            
            if (targetLane == null || targetLane.BoardId != request.BoardId){
                throw new Exception("Lane does not belong to this board");
            }

            var oldCards = await _cardRepository.GetCardsByPositionAsync(card.BoardColumnId, card.LaneId, card.Id);
            for (var i = 0; i < oldCards.Count; i++)
                oldCards[i].Order = i + 1;

            var targetCards = await _cardRepository.GetCardsByPositionAsync(request.TargetBoardColumnId, request.TargetLaneId, card.Id);
            var targetIndex = request.TargetOrder - 1;

            card.BoardColumnId = request.TargetBoardColumnId;
            card.LaneId = request.TargetLaneId;
            targetCards.Insert(targetIndex, card);
            for (var i = 0; i < targetCards.Count; i++)
                targetCards[i].Order = i + 1;

            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
        }
    }
}
