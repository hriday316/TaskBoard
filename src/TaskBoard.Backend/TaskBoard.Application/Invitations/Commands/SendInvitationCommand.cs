using System;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Domain.Enums;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Invitations.Commands;

public class SendInvitationCommand : IRequest<Guid>
{
    public Guid WorkspaceId { get; set; }
    public string InviteeEmail { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IInvitationRepository invitationRepository) : IRequestHandler<SendInvitationCommand, Guid>
    {
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Guid> Handle(SendInvitationCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
                throw new Exception("User not authenticated");
            
            if (request.Role == default)
                request.Role = WorkspaceRole.Member; 

            var invitation = new WorkspaceInvitation
            {
                WorkspaceId = request.WorkspaceId,
                InviteeEmail = request.InviteeEmail,
                Role = request.Role,
                InviteById = Guid.Parse(userId),
                Status = WorkspaceInvitationStatus.Pending,
                SentAt = DateTime.UtcNow
            };
            var result = await _invitationRepository.SendInvitationAsync(invitation);
            return result;
        }
    }

}
