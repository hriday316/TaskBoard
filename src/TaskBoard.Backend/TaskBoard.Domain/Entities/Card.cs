using TaskBoard.Domain.Enums;

namespace TaskBoard.Domain.Entities;

public class Card
{
    public Guid Id { get; set; }

    public Guid BoardColumnId { get; set; }
    public BoardColumn BoardColumn { get; set; } =  null!;

    public Guid LaneId { get; set; }
    public Lane Lane { get; set; } = null!;

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }
    public  CardType CardType { get; set; } = CardType.Task;
    public List<string>? Attachments { get; set; }

     public int Order { get; set; }

    public Guid CreatedByUserId { get; set; }

    public bool IsArchived { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAtUtc { get; set; }

    public List<CardAssignment> Assignments { get; set; } = [];
    public List<Comment> Comments { get; set; } = [];
}
