using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TaskBoard.Domain.Enums;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Application.Invitations.Dto;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Invitations.Commands;

public class UpdateInvitationStatusCommand : IRequest<InvitationDto>
{
    public Guid InvitationId { get; set; }
    public WorkspaceInvitationStatus Status { get; set; }

    public class Handler(IHttpContextAccessor httpContextAccessor, IInvitationRepository invitationRepository, IUserRepository userRepository, IWorkspaceMemberRepository workspaceMemberRepository,IUnitOfWorkRepository unitOfWorkRepository, IMapper mapper) : IRequestHandler<UpdateInvitationStatusCommand, InvitationDto>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository = unitOfWorkRepository;
        private readonly IInvitationRepository _invitationRepository = invitationRepository;
        private readonly IWorkspaceMemberRepository _workspaceMemberRepository = workspaceMemberRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<InvitationDto> Handle(UpdateInvitationStatusCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                throw new Exception("User is not authenticated.");
            }
            var user = await _userRepository.GetCurrentUserAsync(userId) ?? throw new Exception("User not found.");
            var invitation = await _invitationRepository.GetInvitationByIdAsync(request.InvitationId) ?? throw new Exception("Invitation not found.");


            if (user.Email == invitation.InviteeEmail)
            {
                if (invitation.Status != WorkspaceInvitationStatus.Pending)
                {
                    throw new Exception("Invitation is not pending.");
                }

                if (request.Status == WorkspaceInvitationStatus.Accepted)
                {
                    var isMember = await _workspaceMemberRepository.IsWorkspaceMemberAsync(invitation.WorkspaceId, user.Id);
                    if (isMember)
                    {
                        throw new Exception("User is already a member of this workspace.");
                    }

                    invitation.Status = request.Status;
                    invitation.AcceptedAt = DateTime.UtcNow;

                    var workspaceMember = new WorkspaceMember
                    {
                        WorkspaceId = invitation.WorkspaceId,
                        UserId = user.Id,
                        Role = invitation.Role,
                        JoinedAt = DateTime.UtcNow
                    };

                    await _workspaceMemberRepository.AddDefaultWorkspaceMemberAsync(workspaceMember);
                
                }
                else if (request.Status == WorkspaceInvitationStatus.Rejected)
                {
                    invitation.Status = request.Status;
                    invitation.DeclinedAt = DateTime.UtcNow;
                }
            }
            else if (invitation.Workspace.OwnerId == user.Id)
            {
                if (invitation.Status != WorkspaceInvitationStatus.Pending)
                {
                    throw new Exception("Invalid Action");
                }
                if (request.Status == WorkspaceInvitationStatus.Cancelled )
                {
                    invitation.Status = request.Status;
                    invitation.CancelledAt = DateTime.UtcNow;
                }
            }
            else
            {
                throw new Exception("User is not authorized for this action.");
            }

             _invitationRepository.UpdateInvitationStatus(invitation);
            await _unitOfWorkRepository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<InvitationDto>(invitation);



        }
    }

}
