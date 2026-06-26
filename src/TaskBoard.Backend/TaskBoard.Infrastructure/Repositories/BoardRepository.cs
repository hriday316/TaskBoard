using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;
using TaskBoard.Application.Boards.Dto;

namespace TaskBoard.Infrastructure.Repositories;

public class BoardRepository(AppDbContext context) : IBoardRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddBoardAsync(Board board)
    {
        await _context.Boards.AddAsync(board);

    }

    public Task<List<Board>> GetBoardsByWorkspaceIdAsync(Guid workspaceId)
    {
        return _context.Boards.Where(b => b.WorkspaceId == workspaceId && !b.IsArchived).ToListAsync();
    }

    public async Task<Board?> GetBoardByIdAsync(Guid boardId)
    {
        return await _context.Boards.FirstOrDefaultAsync(x => x.Id == boardId);
    }

    public async Task<BoardViewDto?> GetBoardViewAsync(Guid workspaceId, Guid boardId, Guid userId)
    {
        return await _context.Boards
            
            .Where(x => x.Id == boardId && x.WorkspaceId == workspaceId)
            .Where(x => x.Workspace.OwnerId == userId || x.Workspace.Members.Any(m => m.UserId == userId))
            .Select(x => new BoardViewDto
            {
                Id = x.Id,
                WorkspaceId = x.WorkspaceId,
                Name = x.Name ?? string.Empty,

                Columns = x.Columns
                    .Where(c => !c.IsArchived)
                    .OrderBy(c => c.Order)
                    .Select(c => new BoardColumnViewDto
                    {
                        Id = c.Id,
                        BoardId = c.BoardId,
                        Name = c.Name,
                        Order = c.Order,
                        ColumnSpan = c.ColumnSpan,
                        RowSpan = c.RowSpan,
                        Color = c.Color,
                        IsDefault = c.IsDefault,
                        IsArchived = c.IsArchived
                    })
                    .ToList(),

                Lanes = x.Lanes
                    .Where(l => !l.IsArchived)
                    .OrderBy(l => l.Order)
                    .Select(l => new LaneViewDto
                    {
                        Id = l.Id,
                        BoardId = l.BoardId,
                        Name = l.Name,
                        Order = l.Order,
                        Color = l.Color
                    })
                    .ToList(),

                Cells = x.Columns
                    .Where(column => !column.IsArchived)
                    .SelectMany(column => x.Lanes
                        .Where(lane => !lane.IsArchived)
                        .Select(lane => new BoardCellViewDto
                        {
                            ColumnId = column.Id,
                            LaneId = lane.Id,

                            Cards = column.Cards
                                .Where(card =>
                                    !card.IsArchived &&
                                    card.BoardColumnId == column.Id &&
                                    card.LaneId == lane.Id)
                                .OrderBy(card => card.Order)
                                .Select(card => new CardBoardViewDto
                                {
                                    Id = card.Id,
                                    BoardColumnId = card.BoardColumnId,
                                    LaneId = card.LaneId,
                                    Title = card.Title,
                                    Description = card.Description,
                                    Type = card.CardType,
                                    Order = card.Order,
                                    IsArchived = card.IsArchived,
                                    CreatedAtUtc = card.CreatedAtUtc,
                                    UpdatedAtUtc = card.UpdatedAtUtc
                                })
                                .ToList()
                        }))
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }

}

