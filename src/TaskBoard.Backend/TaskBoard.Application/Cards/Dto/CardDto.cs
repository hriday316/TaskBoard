using System;
using TaskBoard.Application.Assignments.Dto;
using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Cards.Dto;

public class CardDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }
    public Guid LaneId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CardType CardType { get; set; } = CardType.Task;
    public List<string>? Attachments { get; set; }
    public List<AssignmentDto> Assignments { get; set; } = [];
    public int Order { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
