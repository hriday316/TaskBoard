namespace TaskBoard.Application.Lanes.Dto;

public class LaneDto
{
    public Guid Id { get; set; }
    public Guid BoardId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }
    public string? Color { get; set; }
    public bool IsDefault { get; set; }
    public bool IsArchived { get; set; }
}
