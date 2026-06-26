using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class BoardColumnRepository(AppDbContext context) : IBoardColumnRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddDefaultColumnsAsync(List<BoardColumn> columns)
    {
        await _context.BoardColumns.AddRangeAsync(columns);
    }

    public async Task<BoardColumn> CreateColumnAsync(BoardColumn column)
    {
        await _context.BoardColumns.AddAsync(column);
        await _context.SaveChangesAsync();
        return column;
    }

    public async Task<List<BoardColumn>> GetColumnsByBoardIdAsync(Guid boardId)
    {
        return await _context.BoardColumns.Where(c => c.BoardId == boardId).ToListAsync();
    }

    public async Task<BoardColumn?> GetColumnByIdAsync(Guid columnId)
    {
        return await _context.BoardColumns.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == columnId);
    }
}
