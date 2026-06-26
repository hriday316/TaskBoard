using TaskBoard.Domain.Enums;

namespace TaskBoard.Application.Boards.Dto;

public class CardBoardViewDto
{
    public Guid Id { get; set; }
    public Guid BoardColumnId { get; set; }
    public Guid LaneId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CardType Type { get; set; }
    public int Order { get; set; }
    public bool IsArchived { get; set; }
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
