using System;

namespace TaskBoard.Application.WorkSpaceMembers.Dto;

public class WorkspaceMemberDto
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string Role { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }

    public DateTime? CreatedAtUtc { get; set; }

}
