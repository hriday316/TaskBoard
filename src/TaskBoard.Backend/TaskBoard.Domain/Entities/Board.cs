using  TaskBoard.Domain.Entities;

namespace TaskBoard.Domain.Entities;

public class Board
{
    public Guid Id { get; set; }

    public Guid WorkspaceId { get; set; }
    public Workspace Workspace { get; set; } =  null!;

    public string? Name { get; set; } 

    public string? Description { get; set; }

     public bool IsArchived { get; set; }
     public bool IsDefault { get; set; }
     public bool IsOpen { get; set; }

    public DateTime? UpdatedAtUtc { get; set; }

    public List<BoardColumn> Columns { get; set; } = [];
    public List<Lane> Lanes { get; set; } = [];
}
