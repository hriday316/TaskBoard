using System;

namespace TaskBoard.Domain.Entities;

public class CardAssignment
{
    public Guid Id { get; set; }
    public Guid CardId { get; set; }
    public Card Card { get; set; } =  null!;

    public Guid WorkspaceMemberId { get; set; }
    public WorkspaceMember WorkspaceMember { get; set; } = null!;
 
    public DateTime AssignedAtUtc { get; set; } = DateTime.UtcNow;
}