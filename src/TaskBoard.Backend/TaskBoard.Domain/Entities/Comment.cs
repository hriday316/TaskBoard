using System;

namespace TaskBoard.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Guid WorkspaceMemberId { get; set; }
    public WorkspaceMember WorkspaceMember { get; set; } = null!;
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }

}
