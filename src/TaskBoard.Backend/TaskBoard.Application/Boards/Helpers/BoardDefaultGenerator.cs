using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Boards.Helpers;

public class BoardDefaultGenerator
{
    public static Board GenerateDefaultBoard(Guid workspaceId)
    {
        return new Board
        {
            Id = Guid.NewGuid(),
            WorkspaceId = workspaceId,
            Name = "Default Board",
            Description = "This is the default board for the workspace.",
            IsDefault = true,
            IsOpen = true
        };
    }

    public static List<BoardColumn> GenerateDefaultColumns(Guid boardId)
    {
        return
        [
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = boardId,
                Name = "To Do",
                Order = 1,
                ColumnSpan = 1,
                RowSpan = 1,
                IsDefault = true
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = boardId,
                Name = "In Progress",
                Order = 2,
                ColumnSpan = 1,
                RowSpan = 1,
                IsDefault = true
            },
            new BoardColumn
            {
                Id = Guid.NewGuid(),
                BoardId = boardId,
                Name = "Done",
                Order = 3,
                ColumnSpan = 1,
                RowSpan = 1,
                IsDefault = true
            }
        ];
    }

    public static Lane GenerateDefaultLane(Guid boardId)
    {
        return new Lane
        {
            Id = Guid.NewGuid(),
            BoardId = boardId,
            Name = "Default",
            Order = 1,
            IsDefault = true
        };
    }
}
