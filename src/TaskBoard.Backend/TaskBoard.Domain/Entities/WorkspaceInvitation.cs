
using  TaskBoard.Domain.Enums;
using TaskBoard.Domain.Entities;
 
namespace TaskBoard.Domain.Entities;

public class WorkspaceInvitation
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } = null!;
    public Guid InviteById { get; set; }
    public ApplicationUser InviteBy { get; set; } = null!;
    public string InviteeEmail { get; set; } = string.Empty;
    public WorkspaceRole Role { get; set; } = WorkspaceRole.Member; 
    public WorkspaceInvitationStatus Status { get; set; } = WorkspaceInvitationStatus.Pending;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public Guid? CancelledById { get; set; }
    public ApplicationUser? CancelledBy { get; set; } = null!;
}