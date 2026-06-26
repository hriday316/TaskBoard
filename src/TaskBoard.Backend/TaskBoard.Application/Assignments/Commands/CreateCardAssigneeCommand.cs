using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Assignments.Dto;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Assignments.Commands;

public class CreateCardAssigneeCommand : IRequest<AssignmentDto>
{
    public Guid CardId { get; set; }
    public Guid MemberId { get; set; }

    public class Handler(ICardAssignmentRepository assignmentRepository, ICardRepository cardRepository, IWorkspaceMemberRepository workspaceMemberRepository, IMapper mapper) : IRequestHandler<CreateCardAssigneeCommand, AssignmentDto>
    {
        private readonly ICardAssignmentRepository _assignmentRepository = assignmentRepository;
        private readonly ICardRepository _cardRepository = cardRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<AssignmentDto> Handle(CreateCardAssigneeCommand request, CancellationToken cancellationToken)
        {
            var card = await _cardRepository.GetCardByIdAsync(request.CardId)
                ?? throw new Exception("Card not found.");

            var members = await _workspaceMemberRepository.GetWorkspaceMembersAsync(card.BoardColumn.Board.WorkspaceId);
            var member = members.FirstOrDefault(x => x.Id == request.MemberId)
                ?? throw new Exception("Member does not belong to the card workspace.");

            var assignment = new CardAssignment
            {
                CardId = request.CardId,
                 WorkspaceMemberId =  request.MemberId
            };

            await _assignmentRepository.AddAssignment(assignment);
            await _assignmentRepository.SaveChangesAsync();

            return _mapper.Map<AssignmentDto>(assignment);
        }
    }


}

      
