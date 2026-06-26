using System;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Invitations.Dto;

namespace TaskBoard.Application.Invitations.Queries;

public class GetMyInvitationQuery: IRequest<List<InvitationDto>>
{
    public class Handler(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IInvitationRepository invitationRepository, IMapper mapper) : IRequestHandler<GetMyInvitationQuery, List<InvitationDto>>
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<List<InvitationDto>> Handle(GetMyInvitationQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("User is not authenticated.");
            }
            var user = await _userRepository.GetCurrentUserAsync(userId);
                if (user == null || string.IsNullOrEmpty(user.Email))
                {
                    throw new Exception("User not found.");
                }

                
            var invitations = await _invitationRepository.GetInvitationsByEmailAsync(user.Email);
            return _mapper.Map<List<InvitationDto>>(invitations);}
    }


}
