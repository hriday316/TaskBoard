namespace TaskBoard.Application.Boards.Dto;

public class BoardViewDto
{
    public Guid Id { get; set; }
    public Guid WorkspaceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<BoardColumnViewDto> Columns { get; set; } = [];
    public List<LaneViewDto> Lanes { get; set; } = [];
    public List<BoardCellViewDto> Cells { get; set; } = [];
}
