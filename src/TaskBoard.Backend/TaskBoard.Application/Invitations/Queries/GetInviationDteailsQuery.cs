using System;
using AutoMapper;
using MediatR;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Invitations.Dto;

namespace TaskBoard.Application.Invitations.Queries;

public class GetInviationDteailsQuery : IRequest<InvitationDto>
{
    public Guid InvitationId { get; set; }
    public class Handler(IInvitationRepository invitationRepository, IMapper mapper) : IRequestHandler<GetInviationDteailsQuery, InvitationDto>
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<InvitationDto> Handle(GetInviationDteailsQuery request, CancellationToken cancellationToken)
        {
             
            var invitation = await _invitationRepository.GetInvitationByIdAsync(request.InvitationId) ?? throw new Exception("Invitation not found");
            return _mapper.Map<InvitationDto>(invitation);
        }
    }

}
