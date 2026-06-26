using System;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Invitations.Dto;

public class InvitationDto
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
        public string? WorkspaceName { get; set; } = string.Empty;
    public string InviteeEmail { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public DateTime? DeclinedAt { get; set; }
    public DateTime? CancelledAt { get; set; }

}
