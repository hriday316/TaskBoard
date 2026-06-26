using TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Entities;

public class Workspace
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; } 
    public Guid OwnerId { get; set; }
    public ApplicationUser Owner { get; set; } = null!;
 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public ICollection<WorkspaceMember> Members { get; set; } = new List<WorkspaceMember>();
    public ICollection<WorkspaceInvitation> Invitations { get; set; } = new List<WorkspaceInvitation>();
    public ICollection<Board> Boards { get; set; } = new List<Board>();
}
 