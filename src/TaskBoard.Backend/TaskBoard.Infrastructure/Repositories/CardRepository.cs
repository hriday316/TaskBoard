using System;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Application.Interfaces.Repositories;
using TaskBoard.Domain.Entities;
using TaskBoard.Infrastructure.Data;

namespace TaskBoard.Infrastructure.Repositories;

public class CardRepository(AppDbContext context) : ICardRepository
{
    private readonly AppDbContext _context = context;
    public async Task AddCard(Card card)
    {
        await _context.Cards.AddAsync(card);
     }

    public async Task<Card?> GetCardByIdAsync(Guid cardId)
    {
        return await _context.Cards
            .AsNoTracking()
            .Include(c => c.BoardColumn)
                .ThenInclude(c => c.Board)
            .Include(c => c.Assignments)
                .ThenInclude(a => a.WorkspaceMember)
                    .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(c => c.Id == cardId);
    }

    public async Task<Card?> GetCardForUpdateAsync(Guid cardId)
    {
        return await _context.Cards
            .Include(x => x.BoardColumn)
                .ThenInclude(x => x.Board)
            .FirstOrDefaultAsync(x => x.Id == cardId);
    }

    public async Task<List<Card>> GetCardsByPositionAsync(Guid columnId, Guid laneId, Guid exceptCardId)
    {
        return await _context.Cards
            .Where(x => x.BoardColumnId == columnId && x.LaneId == laneId && x.Id != exceptCardId && !x.IsArchived)
            .OrderBy(x => x.Order)
            .ToListAsync();
    }

    public async Task<int> GetNextOrderAsync(Guid columnId, Guid laneId)
    {
        return await _context.Cards.CountAsync(x => x.BoardColumnId == columnId && x.LaneId == laneId && !x.IsArchived) + 1;
    }
}
