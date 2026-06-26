using System;
using TaskBoard.Domain.Entities;
using TaskBoard.Application.Boards.Dto;

namespace TaskBoard.Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task AddBoardAsync(Board board);
    Task<Board?> GetBoardByIdAsync(Guid boardId);
    Task<List<Board>> GetBoardsByWorkspaceIdAsync(Guid workspaceId);
    Task<BoardViewDto?> GetBoardViewAsync(Guid workspaceId, Guid boardId, Guid userId);
}
