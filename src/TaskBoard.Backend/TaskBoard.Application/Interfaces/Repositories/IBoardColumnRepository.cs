using System;
using TaskBoard.Domain.Entities;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IBoardColumnRepository
{
    Task AddDefaultColumnsAsync(List<BoardColumn> columns);
    Task<List<BoardColumn>> GetColumnsByBoardIdAsync(Guid boardId);
    Task<BoardColumn?> GetColumnByIdAsync(Guid columnId);
    Task<BoardColumn> CreateColumnAsync(BoardColumn column);

}
