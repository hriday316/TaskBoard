namespace TaskBoard.Application.Boards.Dto;

public class BoardColumnViewDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public int ColumnSpan { get; set; }
    public int RowSpan { get; set; }
    public string? Color { get; set; }
    public bool IsDefault { get; set; }
    public bool IsArchived { get; set; }
 }
