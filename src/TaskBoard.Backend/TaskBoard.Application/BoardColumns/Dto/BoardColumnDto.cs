using System;

namespace TaskBoard.Application.BoardColumns.Dto;

public class BoardColumnDto
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
    public DateTime? CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }

}
