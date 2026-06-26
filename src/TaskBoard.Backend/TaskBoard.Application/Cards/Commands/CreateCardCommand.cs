using System;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Cards.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Cards.Commands;

public class CreateCardCommand : IRequest<CardDto>
{
    public Guid WorkspaceId { get; set; }
    public Guid BoardId { get; set; }
    public Guid BoardColumnId { get; set; }
    public Guid LaneId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? AssignMemberId { get; set; }
    public List<string>? Attachments { get; set; }


    public class Handler(IHttpContextAccessor httpContextAccessor, ICardRepository cardRepository, ICardAssignmentRepository cardAssignmentRepository, IUnitOfWorkRepository unitOfWorkRepository, IWorkspaceMemberRepository workspaceMemberRepository, IBoardRepository boardRepository, IBoardColumnRepository boardColumnRepository, ILaneRepository laneRepository, IMapper mapper) : IRequestHandler<CreateCardCommand, CardDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICardRepository _cardRepository = cardRepository;
        private readonly ICardAssignmentRepository _cardAssignmentRepository = cardAssignmentRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IBoardColumnRepository _boardColumnRepository = boardColumnRepository;
        private readonly IBoardRepository _boardRepository = boardRepository;
        private readonly ILaneRepository _laneRepository = laneRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<CardDto> Handle(CreateCardCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User not authenticated");
             
            var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(request.WorkspaceId, Guid.Parse(userId));
            if (!isMember){
                throw new Exception("Unauthorized user.");
            }
            
            var board = await _boardRepository.GetBoardByIdAsync(request.BoardId);
            if (board == null || board.WorkspaceId != request.WorkspaceId){
                throw new Exception("Board does not found.");
            }
            var columns = await _boardColumnRepository.GetColumnsByBoardIdAsync(request.BoardId);
            if (!columns.Any(x => x.Id == request.BoardColumnId))
            {
                throw new Exception("Column does not belong to this board.");
            }

            var lane = await _laneRepository.GetLaneByIdAsync(request.LaneId);
            if (lane == null || lane.BoardId != request.BoardId){
                throw new Exception("Lane does not belong to this board.");
            }

            if (request.AssignMemberId.HasValue)
            {
                var members = await _workspaceMemberRepository.GetWorkspaceMembersAsync(request.WorkspaceId);
                if (!members.Any(x => x.Id == request.AssignMemberId.Value))
                    throw new Exception("Unauthorize user");
            }
            var card = new Card
            {
                Id = Guid.NewGuid(),
                BoardColumnId = request.BoardColumnId,
                LaneId = request.LaneId,
                Title = request.Title,
                Description = request.Description,
                Order = await _cardRepository.GetNextOrderAsync(request.BoardColumnId, request.LaneId),
                CreatedByUserId = Guid.Parse(userId),
                Attachments = request.Attachments
            };
            if (request.AssignMemberId != null)
            {

                var assignment = new CardAssignment
                {
                    Id = Guid.NewGuid(),
                    CardId = card.Id,
                    WorkspaceMemberId = request.AssignMemberId.Value

                };
                await _cardAssignmentRepository.AddAssignment(assignment);
            }

            await _cardRepository.AddCard(card);
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);

            return _mapper.Map<CardDto>(card);


        }
    }

}
