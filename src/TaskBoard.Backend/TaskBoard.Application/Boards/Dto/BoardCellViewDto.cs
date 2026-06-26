
namespace TaskBoard.Application.Boards.Dto;
public class BoardCellViewDto
{
    public Guid ColumnId { get; set; }
    public Guid LaneId { get; set; }
    public List<CardBoardViewDto> Cards { get; set; } = [];
}