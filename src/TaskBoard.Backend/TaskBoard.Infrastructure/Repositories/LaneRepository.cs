using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class LaneRepository(AppDbContext context) : ILaneRepository
{
    private readonly AppDbContext _context = context;

    public async Task AddLaneAsync(Lane lane)
    {
        await _context.Lanes.AddAsync(lane);
    }

    public async Task<Lane?> GetLaneByIdAsync(Guid laneId)
    {
        return await _context.Lanes.Include(x => x.Board).FirstOrDefaultAsync(x => x.Id == laneId);
    }

    public async Task<List<Lane>> GetLanesByBoardIdAsync(Guid boardId)
    {
        return await _context.Lanes.Where(x => x.BoardId == boardId).ToListAsync();
    }
}
